using HealthChecks.UI.Client;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Configurations;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.Middlewares.ApiInformation;
using LT.DigitalOffice.Kernel.Middlewares.ParseEntities.Models.Requests;
using LT.DigitalOffice.Kernel.Middlewares.ParseEntities.Models.Responses;
using LT.DigitalOffice.Kernel.Middlewares.Token;
using LT.DigitalOffice.MessageService.Broker.Consumers;
using LT.DigitalOffice.MessageService.Broker.Helpers;
using LT.DigitalOffice.MessageService.Broker.Helpers.ParseEntity;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.MessageService.Models.Dto.Configurations;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LT.DigitalOffice.MessageService
{
    public class Startup : BaseApiInfo
    {
        public const string CorsPolicyName = "LtDoCorsPolicy";

        private readonly RabbitMqConfig _rabbitMqConfig;
        private readonly BaseServiceInfoConfig _serviceInfoConfig;

        public IConfiguration Configuration { get; }

        #region private methods

        private void ConfigureMassTransit(IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<SendEmailConsumer>();
                x.AddConsumer<CreateWorkspaceConsumer>();
                x.AddConsumer<UpdateSmtpCredentialsConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(_rabbitMqConfig.Host, "/", host =>
                    {
                        host.Username($"{_serviceInfoConfig.Name}_{_serviceInfoConfig.Id}");
                        host.Password(_serviceInfoConfig.Id);
                    });

                    cfg.ReceiveEndpoint(_rabbitMqConfig.SendEmailEndpoint, ep =>
                    {
                        ep.ConfigureConsumer<SendEmailConsumer>(context);
                    });

                    cfg.ReceiveEndpoint(_rabbitMqConfig.CreateWorkspaceEndpoint, ep =>
                    {
                        ep.ConfigureConsumer<CreateWorkspaceConsumer>(context);
                    });

                    cfg.ReceiveEndpoint(_rabbitMqConfig.UpdateSmtpCredentialsEndpoint, ep =>
                    {
                        ep.ConfigureConsumer<UpdateSmtpCredentialsConsumer>(context);
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

        private void StartResender(IApplicationBuilder app)
        {
            EmailEngineConfig emailEngineConfig = Configuration
                .GetSection(EmailEngineConfig.SectionName)
                .Get<EmailEngineConfig>();

            IServiceProvider serviceProvider = app.ApplicationServices.GetRequiredService<IServiceProvider>();

            var scope = app.ApplicationServices.CreateScope();

            IUnsentEmailRepository repository = scope.ServiceProvider.GetRequiredService<IUnsentEmailRepository>();

            ILoggerFactory loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
            ILogger<EmailResender> logger = loggerFactory.CreateLogger<EmailResender>();

            IRequestClient<IGetSmtpCredentialsRequest> rcGetSmtpCredentials = serviceProvider.CreateRequestClient<IGetSmtpCredentialsRequest>(
                new Uri($"{_rabbitMqConfig.BaseUrl}/{_rabbitMqConfig.GetSmtpCredentialsEndpoint}"), default);

            var resender = new EmailResender(repository, logger, rcGetSmtpCredentials);

            Task.Run(() => resender.StartResend(emailEngineConfig.ResendIntervalInMinutes, emailEngineConfig.MaxResendingCount));
        }

        private void FindParseProperties(IApplicationBuilder app)
        {
            IServiceProvider serviceProvider = app.ApplicationServices.GetRequiredService<IServiceProvider>();

            foreach(KeyValuePair<string, string> pair in _rabbitMqConfig.FindUserParseEntitiesEndpoint)
            {
                IRequestClient<IFindParseEntitiesRequest> rcFindParseEntities = serviceProvider.CreateRequestClient<IFindParseEntitiesRequest>(
                    new Uri($"{_rabbitMqConfig.BaseUrl}/{pair.Value}"), default);

                try
                {
                    var result = rcFindParseEntities.GetResponse<IOperationResult<IFindParseEntitiesResponse>>(IFindParseEntitiesRequest.CreateObj()).Result.Message;

                    if (result.IsSuccess)
                    {
                        AllParseEntities.Entities.Add(pair.Key, result.Body.Entities);
                    }
                }
                catch
                {

                }
            }
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

            Version = "1.3.1.0";
            Description = "MessageService, is intended to work with the messages, emails and email templates.";
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

            services.AddTransient<EmailSender>();

            ConfigureMassTransit(services);
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            UpdateDatabase(app);

            //FindParseProperties(app);

            //StartResender(app);

            app.UseForwardedHeaders();

            app.UseExceptionsHandler(loggerFactory);

            app.UseApiInformation();

            app.UseRouting();

            app.UseMiddleware<TokenMiddleware>();

            app.UseCors(CorsPolicyName);

            app.UseEndpoints(endpoints =>
            {
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
