using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Db.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.Models.Broker.Requests.Message;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace LT.DigitalOffice.MessageService.Broker.Consumers
{
    public class SendEmailConsumer : IConsumer<ISendEmailRequest>
    {
        private readonly IDbEmailMapper _mapper;
        private readonly IEmailRepository _emailRepository;
        private readonly ILogger<SendEmailConsumer> _logger;
        private readonly IEmailTemplateRepository _templateRepository;

        private readonly IOptions<SmtpCredentialsOptions> _options;

        private bool SendEmail(ISendEmailRequest request)
        {
            _logger.LogInformation(
                "Start email sending to '{receiver}'.",
                request.Email);

            MailAddress from = new(_options.Value.Email);
            MailAddress to = new(request.Email);

            var dbEmailTemplateText = GetDbEmailTemplateText(request);
            string subject = GetParsedEmailTemplateText(request.TemplateValues, dbEmailTemplateText.Subject);
            string body = GetParsedEmailTemplateText(request.TemplateValues, dbEmailTemplateText.Text);

            var message = new MailMessage(from, to)
            {
                Subject = GetParsedEmailTemplateText(request.TemplateValues, dbEmailTemplateText.Subject),
                Body = GetParsedEmailTemplateText(request.TemplateValues, dbEmailTemplateText.Text)
            };

            var smtp = new SmtpClient(_options.Value.Host, _options.Value.Port)
            {
                Credentials = new NetworkCredential(_options.Value.Email, _options.Value.Password),
                EnableSsl = true
            };

            try
            {
                smtp.Send(message);

                _logger.LogInformation(
                    "Email '{subject}' was sent successfully to '{receiver}'.",
                    subject,
                    to.Address);
            }
            catch (Exception ex)
            {
                _logger.LogError("Email was not send. Reason: '{error}'.", ex);

                throw;
            }

            var dbEmail = _mapper.Map(request);
            dbEmail.Body = body;
            dbEmail.Subject = subject;

            _emailRepository.SaveEmail(dbEmail);

            return true;
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
            IDbEmailMapper mapper,
            IEmailRepository emailRepository,
            ILogger<SendEmailConsumer> logger,
            IOptions<SmtpCredentialsOptions> options,
            IEmailTemplateRepository templateRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _options = options;
            _emailRepository = emailRepository;
            _templateRepository = templateRepository;
        }

        public async Task Consume(ConsumeContext<ISendEmailRequest> context)
        {
            var response = OperationResultWrapper.CreateResponse(SendEmail, context.Message);

            await context.RespondAsync<IOperationResult<bool>>(response);
        }
    }
}
