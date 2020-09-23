using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace LT.DigitalOffice.MessageService.Broker.Consumers
{
    public class SendEmailConsumer : IConsumer<ISendEmailRequest>
    {
        private readonly IEmailRepository repository;
        private readonly IMapper<ISendEmailRequest, DbEmail> mapper;

        public SendEmailConsumer([FromServices] IEmailRepository repository, IMapper<ISendEmailRequest, DbEmail> mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task Consume(ConsumeContext<ISendEmailRequest> context)
        {
            var response = OperationResultWrapper.CreateResponse(SendEmail, context.Message);

            await context.RespondAsync<IOperationResult<bool>>(response);
        }

        private bool SendEmail(ISendEmailRequest request)
        {
            MailAddress from = new MailAddress("er0289741@gmail.com");
            MailAddress to = new MailAddress(request.Receiver);

            var m = new MailMessage(from, to)
            {
                Subject = request.Subject,
                Body = request.Body
            };

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("er0289741@gmail.com", "er0289741123456"),
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