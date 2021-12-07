using System;
using System.Collections.Generic;
using HealthChecks.UI.Client;
using LT.DigitalOffice.Kernel.BrokerSupport.Configurations;
using LT.DigitalOffice.Kernel.BrokerSupport.Extensions;
using LT.DigitalOffice.Kernel.BrokerSupport.Middlewares.Token;
using LT.DigitalOffice.Kernel.Configurations;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.Middlewares.ApiInformation;
using LT.DigitalOffice.MessageService.Broker.Consumers;
using LT.DigitalOffice.MessageService.Business.Commands.Message.Hubs;
using LT.DigitalOffice.MessageService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.MessageService.Models.Dto.Configurations;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Serilog;

namespace LT.DigitalOffice.MessageService
{
  public class Startup : BaseApiInfo
  {
    public const string CorsPolicyName = "LtDoCorsPolicy";

    private readonly RabbitMqConfig _rabbitMqConfig;
    private readonly BaseServiceInfoConfig _serviceInfoConfig;

    public IConfiguration Configuration { get; }

    #region private methods

    private (string username, string password) GetRabbitMqCredentials()
    {
      static string GetString(string envVar, string formAppsettings, string generated, string fieldName)
      {
        string str = Environment.GetEnvironmentVariable(envVar);
        if (string.IsNullOrEmpty(str))
        {
          str = formAppsettings ?? generated;

          Log.Information(
            formAppsettings == null
              ? $"Default RabbitMq {fieldName} was used."
              : $"RabbitMq {fieldName} from appsetings.json was used.");
        }
        else
        {
          Log.Information($"RabbitMq {fieldName} from environment was used.");
        }

        return str;
      }

      return (GetString("RabbitMqUsername", _rabbitMqConfig.Username, $"{_serviceInfoConfig.Name}_{_serviceInfoConfig.Id}", "Username"),
        GetString("RabbitMqPassword", _rabbitMqConfig.Password, _serviceInfoConfig.Id, "Password"));
    }

    private void ConfigureMassTransit(IServiceCollection services)
    {
      (string username, string password) = GetRabbitMqCredentials();

      services.AddMassTransit(x =>
      {
        x.AddConsumer<CreateWorkspaceConsumer>();

        x.UsingRabbitMq((context, cfg) =>
        {
          cfg.Host(_rabbitMqConfig.Host, "/", host =>
          {
            host.Username(username);
            host.Password(password);
          });

          cfg.ReceiveEndpoint(_rabbitMqConfig.CreateWorkspaceEndpoint, ep =>
          {
            ep.ConfigureConsumer<CreateWorkspaceConsumer>(context);
          });
        });

        x.AddRequestClients(_rabbitMqConfig);
      });

      services.AddMassTransitHostedService();
    }

    private void UpdateDatabase(IApplicationBuilder app)
    {
      using var serviceScope = app.ApplicationServices
        .GetRequiredService<IServiceScopeFactory>()
        .CreateScope();

      using var context = serviceScope.ServiceProvider.GetService<MessageServiceDbContext>();

      context.Database.Migrate();
    }

    #endregion

    #region public methods

    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;

      _serviceInfoConfig = Configuration
        .GetSection(BaseServiceInfoConfig.SectionName)
        .Get<BaseServiceInfoConfig>();

      _rabbitMqConfig = Configuration
        .GetSection(BaseRabbitMqConfig.SectionName)
        .Get<RabbitMqConfig>();

      Version = "1.3.2.1";
      Description = "MessageService, is intended to work with the messages.";
      StartTime = DateTime.UtcNow;
      ApiName = $"LT Digital Office - {_serviceInfoConfig.Name}";
    }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddCors(options =>
      {
        options.AddPolicy(
          CorsPolicyName,
          builder =>
          {
            builder
              .AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
          });
      });

      string connStr = Environment.GetEnvironmentVariable("ConnectionString");
      if (string.IsNullOrEmpty(connStr))
      {
        connStr = Configuration.GetConnectionString("SQLConnectionString");
      }

      services.Configure<TokenConfiguration>(Configuration.GetSection("CheckTokenMiddleware"));
      services.Configure<BaseRabbitMqConfig>(Configuration.GetSection(BaseRabbitMqConfig.SectionName));
      services.Configure<BaseServiceInfoConfig>(Configuration.GetSection(BaseServiceInfoConfig.SectionName));

      services.AddHttpContextAccessor();

      services.AddSignalR();

      services.AddDbContext<MessageServiceDbContext>(options =>
      {
        options.UseSqlServer(connStr);
      });

      services
        .AddHealthChecks()
        .AddSqlServer(connStr)
        .AddRabbitMqCheck();

      services
        .AddControllers()
        .AddNewtonsoftJson();

      services.AddBusinessObjects();

      ConfigureMassTransit(services);
    }

    public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
    {
      UpdateDatabase(app);

      app.UseForwardedHeaders();

      app.UseExceptionsHandler(loggerFactory);

      app.UseApiInformation();

      app.UseRouting();

      app.UseMiddleware<TokenMiddleware>();

      app.UseCors(CorsPolicyName);

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapHub<ChatHub>("/chatHub");

        endpoints.MapControllers().RequireCors(CorsPolicyName);

        endpoints.MapHealthChecks($"/{_serviceInfoConfig.Id}/hc", new HealthCheckOptions
        {
          ResultStatusCodes = new Dictionary<HealthStatus, int>
          {
            { HealthStatus.Unhealthy, 200 },
            { HealthStatus.Healthy, 200 },
            { HealthStatus.Degraded, 200 },
          },
          Predicate = check => check.Name != "masstransit-bus",
          ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
      });
    }

    #endregion
  }
}
