using LT.DigitalOffice.Broker.Requests;
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
        private readonly IMessageRepository repository;
        private readonly IMapper<Message, DbMessage> mapper;

        public SendEmailConsumer([FromServices] IMessageRepository repository, IMapper<Message, DbMessage> mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task Consume(ConsumeContext<ISendEmailRequest> context)
        {
            await SendEmail(context.Message);
        }

        private async Task SendEmail(ISendEmailRequest request)
        {
            MailAddress from = new MailAddress(request.SenderEmail);
            MailAddress to = new MailAddress(request.RecipientEmail);
            var m = new MailMessage(from, to)
            {
                Subject = request.Title,
                Body = request.Content
            };

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("er0289741@gmail.com", "er0289741123456"),
                EnableSsl = true
            };
            await smtp.SendMailAsync(m);

            var message = new Message
            {
                Title = request.Title,
                Content = request.Content
            };

            repository.SaveMessage(mapper.Map(message));
        }
    }
}