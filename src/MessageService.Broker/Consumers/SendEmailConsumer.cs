using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace LT.DigitalOffice.MessageService.Broker.Consumers
{
    public class SendEmailConsumer : IConsumer<ISendEmailRequest>
    {
        private readonly IEmailRepository repository;
        private readonly IMapper<ISendEmailRequest, DbEmail> mapper;
        private readonly IOptions<SmtpCredentialsOptions> options;

        public SendEmailConsumer(
            [FromServices] IEmailRepository repository,
            [FromServices] IMapper<ISendEmailRequest, DbEmail> mapper,
            [FromServices] IOptions<SmtpCredentialsOptions> options)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.options = options;
        }

        public async Task Consume(ConsumeContext<ISendEmailRequest> context)
        {
            var response = OperationResultWrapper.CreateResponse(SendEmail, context.Message);

            await context.RespondAsync<IOperationResult<bool>>(response);
        }

        private bool SendEmail(ISendEmailRequest request)
        {
            MailAddress from = new MailAddress(options.Value.Email);
            MailAddress to = new MailAddress(request.Receiver);

            var m = new MailMessage(from, to)
            {
                Subject = request.Subject,
                Body = request.Body
            };

            SmtpClient smtp = new SmtpClient(options.Value.Host, options.Value.Port)
            {
                Credentials = new NetworkCredential(options.Value.Email, options.Value.Password),
                EnableSsl = true
            };

            smtp.Send(m);

            SaveEmail(request);

            return true;
        }

        private void SaveEmail(ISendEmailRequest request)
        {
            repository.SaveEmail(mapper.Map(request));
        }
    }
}