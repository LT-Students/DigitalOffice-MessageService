using FluentValidation;
using LT.DigitalOffice.Kernel;
using LT.DigitalOffice.Kernel.Broker;
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
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            services.Configure<RabbitMQOptions>(Configuration);

            services.AddHealthChecks();

            services.AddDbContext<MessageServiceDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("SQLConnectionString"));
            });

            services.AddControllers();

            ConfigureCommands(services);
            ConfigureMappers(services);
            ConfigureRepositories(services);
            ConfigureMassTransit(services);
            ConfigureValidators(services);

            services.AddKernelExtensions(Configuration);
        }

        private void ConfigureMassTransit(IServiceCollection services)
        {
            const string serviceSection = "RabbitMQ";
            string serviceName = Configuration.GetSection(serviceSection)["Username"];
            string servicePassword = Configuration.GetSection(serviceSection)["Password"];

            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("localhost", "/", host =>
                    {
                        host.Username($"{serviceName}_{servicePassword}");
                        host.Password(servicePassword);
                    });
                });
            });

            services.AddMassTransitHostedService();
        }

        private void ConfigureMappers(IServiceCollection services)
        {
            services.AddTransient<IMapper<EmailTemplate, DbEmailTemplate>, EmailTemplateMapper>();
            services.AddTransient<IMapper<EditEmailTemplateRequest, DbEmailTemplate, DbEmailTemplate>, EmailTemplateMapper>();
        }

        private void ConfigureCommands(IServiceCollection services)
        {
            services.AddTransient<IAddEmailTemplateCommand, AddEmailTemplateCommand>();
        }

        private void ConfigureRepositories(IServiceCollection services)
        {
            services.AddTransient<IDataProvider, MessageServiceDbContext>();

            services.AddTransient<IEmailTemplateRepository, EmailTemplateRepository>();
        }

        private void ConfigureValidators(IServiceCollection services)
        {
            services.AddTransient<IValidator<EditEmailTemplateRequest>, EditEmailTemplateValidator>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseHealthChecks("/api/healthcheck");

            app.UseExceptionHandler(tempApp => tempApp.Run(CustomExceptionHandler.HandleCustomException));

            UpdateDatabase(app);

            app.UseHttpsRedirection();
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
