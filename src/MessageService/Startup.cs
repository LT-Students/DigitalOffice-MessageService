using FluentValidation;
using HealthChecks.UI.Client;
using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.Kernel.Configurations;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.Middlewares.ApiInformation;
using LT.DigitalOffice.Kernel.Middlewares.Token;
using LT.DigitalOffice.MessageService.Broker.Consumers;
using LT.DigitalOffice.MessageService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.MessageService.Mappers.EmailMappers;
using LT.DigitalOffice.MessageService.Mappers.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Configurations;
using LT.DigitalOffice.MessageService.Models.Dto.Models;
using LT.DigitalOffice.MessageService.Models.Dto.Requests;
using LT.DigitalOffice.MessageService.Validation;
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

namespace LT.DigitalOffice.MessageService
{
    public class Startup : BaseApiInfo
    {
        public const string CorsPolicyName = "LtDoCorsPolicy";

        private readonly RabbitMqConfig _rabbitMqConfig;
        private readonly BaseServiceInfoConfig _serviceInfoConfig;

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            _serviceInfoConfig = Configuration
                .GetSection(BaseServiceInfoConfig.SectionName)
                .Get<BaseServiceInfoConfig>();

            _rabbitMqConfig = Configuration
                .GetSection(BaseRabbitMqConfig.SectionName)
                .Get<RabbitMqConfig>();

            Version = "1.2.5";
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
                            .WithOrigins(
                                "https://*.ltdo.xyz",
                                "http://*.ltdo.xyz",
                                "http://ltdo.xyz",
                                "http://ltdo.xyz:9802",
                                "http://localhost:4200",
                                "http://localhost:4500")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            var smtpCredentialsOptions = Configuration.GetSection(SmtpCredentialsOptions.SmtpCredentials);

            var smtpHost = Environment.GetEnvironmentVariable(nameof(SmtpCredentialsOptions.Host));
            if (!string.IsNullOrEmpty(smtpHost))
            {
                smtpCredentialsOptions[nameof(SmtpCredentialsOptions.Host)] = smtpHost;
            }

            var smtpPort = Environment.GetEnvironmentVariable(nameof(SmtpCredentialsOptions.Port));
            if (!string.IsNullOrEmpty(smtpPort))
            {
                smtpCredentialsOptions[nameof(SmtpCredentialsOptions.Port)] = smtpPort;
            }

            var smtpEmail = Environment.GetEnvironmentVariable(nameof(SmtpCredentialsOptions.Email));
            if (!string.IsNullOrEmpty(smtpEmail))
            {
                smtpCredentialsOptions[nameof(SmtpCredentialsOptions.Email)] = smtpEmail;
            }

            var smtpPassword = Environment.GetEnvironmentVariable(nameof(SmtpCredentialsOptions.Password));
            if (!string.IsNullOrEmpty(smtpPassword))
            {
                smtpCredentialsOptions[nameof(SmtpCredentialsOptions.Password)] = smtpPassword;
            }

            string connStr = Environment.GetEnvironmentVariable("ConnectionString");
            if (string.IsNullOrEmpty(connStr))
            {
                connStr = Configuration.GetConnectionString("SQLConnectionString");
            }

            services.Configure<SmtpCredentialsOptions>(smtpCredentialsOptions);
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

            services.AddControllers();
            services.AddBusinessObjects();

            ConfigureMassTransit(services);

            ConfigureMappers(services);
            ConfigureValidators(services);
        }

        private void ConfigureMassTransit(IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<SendEmailConsumer>();
                x.AddConsumer<EmailTemplateTagsConsumer>();

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

                    cfg.ReceiveEndpoint(_rabbitMqConfig.GetTempalateTagsEndpoint, ep =>
                    {
                        ep.ConfigureConsumer<EmailTemplateTagsConsumer>(context);
                    });
                });

                x.AddRequestClients(_rabbitMqConfig);
            });

            services.AddMassTransitHostedService();
        }

        private void ConfigureMappers(IServiceCollection services)
        {
            services.AddTransient<IMapper<ISendEmailRequest, DbEmail>, EmailMapper>();
            services.AddTransient<IMapper<EmailTemplateRequest, DbEmailTemplate>, EmailTemplateMapper>();
            services.AddTransient<IMapper<EmailTemplateTextInfo, DbEmailTemplateText>, EmailTemplateTextMapper>();
            services.AddTransient<IMapper<EditEmailTemplateRequest, DbEmailTemplate>, EmailTemplateMapper>();
        }

        private void ConfigureValidators(IServiceCollection services)
        {
            services.AddTransient<IValidator<EditEmailTemplateRequest>, EditEmailTemplateValidator>();
            services.AddTransient<IValidator<Workspace>, WorkspaceValidator>();
            services.AddTransient<IValidator<EmailTemplateRequest>, AddEmailTemplateValidator>();
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

        private void UpdateDatabase(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();

            using var context = serviceScope.ServiceProvider.GetService<MessageServiceDbContext>();

            context.Database.Migrate();
        }
    }
}
