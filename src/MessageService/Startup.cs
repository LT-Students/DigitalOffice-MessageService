using FluentValidation;
using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.Kernel;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Middlewares.Token;
using LT.DigitalOffice.MessageService.Broker.Consumers;
using LT.DigitalOffice.MessageService.Business.EmailTemplatesCommands;
using LT.DigitalOffice.MessageService.Business.EmailTemplatesCommands.Interfaces;
using LT.DigitalOffice.MessageService.Business.WorkspaceCommands;
using LT.DigitalOffice.MessageService.Business.WorkspaceCommands.Interfaces;
using LT.DigitalOffice.MessageService.Data;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Data.Provider;
using LT.DigitalOffice.MessageService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.MessageService.Mappers.EmailMappers;
using LT.DigitalOffice.MessageService.Mappers.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.WorkspaceMappers;
using LT.DigitalOffice.MessageService.Mappers.WorkspaceMappers.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Models;
using LT.DigitalOffice.MessageService.Models.Dto.Requests;
using LT.DigitalOffice.MessageService.Models.Dto.Responses;
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
            services.AddKernelExtensions();

            services.Configure<SmtpCredentialsOptions>(Configuration.GetSection(SmtpCredentialsOptions.SmtpCredentials));

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

            ConfigureMassTransit(services);

            ConfigureCommands(services);
            ConfigureMappers(services);
            ConfigureRepositories(services);
            ConfigureValidators(services);
        }

        private void ConfigureMassTransit(IServiceCollection services)
        {
            var rabbitMqConfig = Configuration
                .GetSection(BaseRabbitMqOptions.RabbitMqSectionName)
                .Get<RabbitMqConfig>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<SendEmailConsumer>();
                x.AddConsumer<EmailTemplateTagsConsumer>();

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

                    cfg.ReceiveEndpoint(rabbitMqConfig.GetTempalateTagsEndpoint, ep =>
                    {
                        ep.ConfigureConsumer<EmailTemplateTagsConsumer>(context);
                    });
                });

                x.AddRequestClient<ICheckTokenRequest>(
                    new Uri($"{rabbitMqConfig.BaseUrl}/{rabbitMqConfig.ValidateTokenEndpoint}"));
                x.AddRequestClient<IAddImageRequest>(
                    new Uri($"{rabbitMqConfig.BaseUrl}/{rabbitMqConfig.CreateImageEndpoint}"));
                x.AddRequestClient<IAddImageRequest>(
                    new Uri($"{rabbitMqConfig.BaseUrl}/{rabbitMqConfig.GetTempalateTagsEndpoint}"));

                x.ConfigureKernelMassTransit(rabbitMqConfig);
            });

            services.AddMassTransitHostedService();
        }

        private void ConfigureMappers(IServiceCollection services)
        {
            services.AddTransient<IDbWorkspaceMapper, DbWorkspaceMapper>();
            services.AddTransient<IMapper<ISendEmailRequest, DbEmail>, EmailMapper>();
            services.AddTransient<IMapper<EmailTemplateRequest, DbEmailTemplate>, EmailTemplateMapper>();
            services.AddTransient<IMapper<EmailTemplateTextInfo, DbEmailTemplateText>, EmailTemplateTextMapper>();
            services.AddTransient<IMapper<EditEmailTemplateRequest, DbEmailTemplate>, EmailTemplateMapper>();
        }

        private void ConfigureCommands(IServiceCollection services)
        {
            services.AddTransient<IDisableEmailTemplateCommand, DisableEmailTemplateCommand>();
            services.AddTransient<IAddEmailTemplateCommand, AddEmailTemplateCommand>();
            services.AddTransient<IEditEmailTemplateCommand, EditEmailTemplateCommand>();
            services.AddTransient<ICreateWorkspaceCommand, CreateWorkspaceCommand>();
            services.AddTransient<IRemoveWorkspaceCommand, RemoveWorkspaceCommand>();
        }

        private void ConfigureRepositories(IServiceCollection services)
        {
            services.AddTransient<IDataProvider, MessageServiceDbContext>();

            services.AddTransient<IWorkspaceRepository, WorkspaceRepository>();
            services.AddTransient<IEmailRepository, EmailRepository>();
            services.AddTransient<IEmailTemplateRepository, EmailTemplateRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
        }

        private void ConfigureValidators(IServiceCollection services)
        {
            services.AddTransient<IValidator<EditEmailTemplateRequest>, EditEmailTemplateValidator>();
            services.AddTransient<IValidator<Workspace>, WorkspaceValidator>();
            services.AddTransient<IValidator<EmailTemplateRequest>, AddEmailTemplateValidator>();
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
