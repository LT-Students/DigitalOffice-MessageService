using FluentValidation;
using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.Kernel;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Middlewares.Token;
using LT.DigitalOffice.MessageService.Broker.Consumers;
using LT.DigitalOffice.MessageService.Business;
using LT.DigitalOffice.MessageService.Business.Interfaces;
using LT.DigitalOffice.MessageService.Data;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Data.Provider;
using LT.DigitalOffice.MessageService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.MessageService.Mappers;
using LT.DigitalOffice.MessageService.Mappers.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto;
using LT.DigitalOffice.MessageService.Validation;
using LT.DigitalOffice.UserService.Configuration;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace LT.DigitalOffice.MessageService
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<SmtpCredentialsOptions>(Configuration);

            services.AddHealthChecks();

            string connStr = Environment.GetEnvironmentVariable("ConnectionString");
            if (string.IsNullOrEmpty(connStr))
            {
                connStr = Configuration.GetConnectionString("SQLConnectionString");
            }

            services.AddDbContext<MessageServiceDbContext>(options =>
            {
                options.UseSqlServer(connStr);
            });

            services.AddControllers();

            ConfigureCommands(services);
            ConfigureMappers(services);
            ConfigureRepositories(services);
            ConfigureValidators(services);
            ConfigureMassTransit(services);

            services.AddKernelExtensions();
        }

        private void ConfigureMassTransit(IServiceCollection services)
        {
            var rabbitMqConfig = Configuration
                .GetSection(BaseRabbitMqOptions.RabbitMqSectionName)
                .Get<RabbitMqConfig>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<SendEmailConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitMqConfig.Host, "/", host =>
                    {
                        host.Username($"{rabbitMqConfig.Username}_{rabbitMqConfig.Password}");
                        host.Password(rabbitMqConfig.Password);
                    });

                    cfg.ReceiveEndpoint(rabbitMqConfig.SendEmailEndpoint, ep =>
                    {
                        ep.ConfigureConsumer<SendEmailConsumer>(context);
                    });

                    x.AddRequestClient<ICheckTokenRequest>(
                        new Uri($"{rabbitMqConfig.BaseUrl}/{rabbitMqConfig.ValidateTokenEndpoint}"));
                    x.AddRequestClient<ICreateImageRequest>(
                        new Uri($"{rabbitMqConfig.BaseUrl}/{rabbitMqConfig.SendEmailEndpoint}"));

                    x.ConfigureKernelMassTransit(rabbitMqConfig);
                });

                x.ConfigureKernelMassTransit(rabbitMqConfig);
            });

            services.AddMassTransitHostedService();
        }

        private void ConfigureMappers(IServiceCollection services)
        {
            services.AddTransient<IMapper<Workspace, DbWorkspace>, WorkspaceMapper>();
            services.AddTransient<IMapper<ISendEmailRequest, DbEmail>, EmailMapper>();
            services.AddTransient<IMapper<EmailTemplate, DbEmailTemplate>, EmailTemplateMapper>();
            services.AddTransient<IMapper<EditEmailTemplateRequest, DbEmailTemplate>, EmailTemplateMapper>();
        }

        private void ConfigureCommands(IServiceCollection services)
        {
            services.AddTransient<IDisableEmailTemplateCommand, DisableEmailTemplateCommand>();
            services.AddTransient<IAddEmailTemplateCommand, AddEmailTemplateCommand>();
            services.AddTransient<IEditEmailTemplateCommand, EditEmailTemplateCommand>();
            services.AddTransient<ICreateWorkspaceCommand, CreateWorkspaceCommand>();
        }

        private void ConfigureRepositories(IServiceCollection services)
        {
            services.AddTransient<IDataProvider, MessageServiceDbContext>();

            services.AddTransient<IWorkspaceRepository, WorkspaceRepository>();
            services.AddTransient<IEmailRepository, EmailRepository>();
            services.AddTransient<IEmailTemplateRepository, EmailTemplateRepository>();
        }

        private void ConfigureValidators(IServiceCollection services)
        {
            services.AddTransient<IValidator<EditEmailTemplateRequest>, EditEmailTemplateValidator>();
            services.AddTransient<IValidator<Workspace>, AddWorkspaceRequestValidator>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseHealthChecks("/api/healthcheck");

            app.UseExceptionHandler(tempApp => tempApp.Run(CustomExceptionHandler.HandleCustomException));

            UpdateDatabase(app);

            app.UseMiddleware<TokenMiddleware>();

#if RELEASE
            app.UseHttpsRedirection();
#endif

            app.UseRouting();

            string corsUrl = Configuration.GetSection("Settings")["CorsUrl"];

            app.UseCors(builder =>
                builder
                    .WithOrigins(corsUrl)
                    .AllowAnyHeader()
                    .AllowAnyMethod());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
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
