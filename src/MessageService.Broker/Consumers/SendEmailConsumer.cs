using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.MessageService.Data.Interfaces;
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

        public SendEmailConsumer([FromServices] IMessageRepository repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<ISendEmailRequest> context)
        {
            await SendEmailAsync(context.Message);
        }

        private static async Task SendEmailAsync(ISendEmailRequest request)
        {
            MailAddress from = new MailAddress(request.SenderEmail);
            MailAddress to = new MailAddress(request.RecipientEmail);
            var m = new MailMessage(from, to)
            {
                Subject = request.Title,
                Body = request.Content
            };

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("somemail@gmail.com", "mypassword");
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(m);
        }
    }
}
