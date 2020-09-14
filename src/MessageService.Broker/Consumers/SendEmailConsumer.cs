using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.Broker.Responses;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace LT.DigitalOffice.MessageService.Broker.Consumers
{
    public class SendEmailConsumer : IConsumer<ISendEmailRequest>
    {
        private readonly IRequestClient<IGetUserIdByEmailRequest> client;
        private readonly IMessageRepository repository;
        private readonly IMapper<Message, DbMessage> mapper;

        public SendEmailConsumer(
            [FromServices] IMessageRepository repository,
            [FromServices] IRequestClient<IGetUserIdByEmailRequest> client,
            [FromServices] IMapper<Message, DbMessage> mapper)
        {
            this.repository = repository;
            this.client = client;
            this.mapper = mapper;
        }

        public async Task Consume(ConsumeContext<ISendEmailRequest> context)
        {
            var response = OperationResultWrapper.CreateResponse(SendEmail, context.Message);

            await context.RespondAsync<IOperationResult<bool>>(response);
        }

        private bool SendEmail(ISendEmailRequest request)
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
            smtp.Send(m);

            SaveMessage(request);

            return true;
        }

        private void SaveMessage(ISendEmailRequest request)
        {
            var message = new Message
            {
                Title = request.Title,
                Content = request.Content,
                SenderUserId = GetUserIdByEmail(request.SenderEmail),
                RecipientUserId = GetUserIdByEmail(request.RecipientEmail)
            };

            repository.SaveMessage(mapper.Map(message));
        }

        private Guid GetUserIdByEmail(string email)
        {
            var response = client.GetResponse<IOperationResult<IGetUserIdByEmailResponse>>(new
            {
                UserEmail = email
            }).Result;

            if (!response.Message.IsSuccess)
            {
                throw new Exception(string.Join(", ", response.Message.Errors));
            }

            return response.Message.Body.UserId;
        }
    }
}