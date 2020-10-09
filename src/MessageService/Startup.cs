using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.Kernel;
using LT.DigitalOffice.Kernel.Broker;
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
            services.Configure<SmtpCredentialsOptions>(Configuration);

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

            services.AddKernelExtensions();
        }

        private void ConfigureMassTransit(IServiceCollection services)
        {
            var rabbitMQOptions = Configuration.GetSection(RabbitMQOptions.RabbitMQ).Get<RabbitMQOptions>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<SendEmailConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitMQOptions.Host, "/", host =>
                    {
                        host.Username($"{rabbitMQOptions.Username}_{rabbitMQOptions.Password}");
                        host.Password(rabbitMQOptions.Password);
                    });

                    cfg.ReceiveEndpoint(rabbitMQOptions.Username, ep =>
                    {
                        ep.ConfigureConsumer<SendEmailConsumer>(context);
                    });
                });

                x.ConfigureKernelMassTransit(rabbitMQOptions);
            });

            services.AddMassTransitHostedService();
        }

        private void ConfigureMappers(IServiceCollection services)
        {
            services.AddTransient<IMapper<ISendEmailRequest, DbEmail>, EmailMapper>();
            services.AddTransient<IMapper<EmailTemplate, DbEmailTemplate>, EmailTemplateMapper>();
        }

        private void ConfigureCommands(IServiceCollection services)
        {
            services.AddTransient<IDisableEmailTemplateCommand, DisableEmailTemplateCommand>();
            services.AddTransient<IAddEmailTemplateCommand, AddEmailTemplateCommand>();
        }

        private void ConfigureRepositories(IServiceCollection services)
        {
            services.AddTransient<IDataProvider, MessageServiceDbContext>();

            services.AddTransient<IEmailRepository, EmailRepository>();
            services.AddTransient<IEmailTemplateRepository, EmailTemplateRepository>();
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
