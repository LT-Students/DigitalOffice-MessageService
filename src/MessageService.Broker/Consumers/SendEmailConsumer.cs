using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.UserService.Business.Helpers.Email;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.DigitalOffice.MessageService.Broker.Consumers
{
    public class SendEmailConsumer : IConsumer<ISendEmailRequest>
    {
        private readonly ILogger<SendEmailConsumer> _logger;
        private readonly IEmailTemplateRepository _templateRepository;
        private readonly EmailSender _sender;

        private bool SendEmail(ISendEmailRequest request)
        {
            var dbEmailTemplateText = GetDbEmailTemplateText(request);
            string subject = GetParsedEmailTemplateText(request.TemplateValues, dbEmailTemplateText.Subject);
            string body = GetParsedEmailTemplateText(request.TemplateValues, dbEmailTemplateText.Text);

            return _sender.SendEmail(request.Email, subject, body);
        }

        private DbEmailTemplateText GetDbEmailTemplateText(ISendEmailRequest request)
        {
            var dbEmailTemplate = _templateRepository.GetEmailTemplateById(request.TemplateId);

            var dbEmailTemplateText = dbEmailTemplate?.EmailTemplateTexts.FirstOrDefault(ett => ett.Language == request.Language);

            if (dbEmailTemplateText == null)
            {
                string messageTemp = "Email template text was not found.";
                _logger.LogWarning(messageTemp);

                throw new NotFoundException(messageTemp);
            }

            return dbEmailTemplateText;
        }

        private string GetParsedEmailTemplateText(IDictionary<string, string> values, string text)
        {
            string[] strArray = text.Split('{', '}');

            for (int i = 0; i < strArray.Length; i++)
            {
                if (values.TryGetValue(strArray[i], out string value))
                {
                    strArray[i] = value;
                }
            }

            StringBuilder sb = new();
            sb.AppendJoin("", strArray);

            return sb.ToString();
        }

        public SendEmailConsumer(
            ILogger<SendEmailConsumer> logger,
            IEmailTemplateRepository templateRepository,
            EmailSender sender)
        {
            _logger = logger;
            _templateRepository = templateRepository;
            _sender = sender;
        }

        public async Task Consume(ConsumeContext<ISendEmailRequest> context)
        {
            var response = OperationResultWrapper.CreateResponse(SendEmail, context.Message);

            await context.RespondAsync<IOperationResult<bool>>(response);
        }
    }
}
