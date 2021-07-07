using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Helpers;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using LT.DigitalOffice.Models.Broker.Responses.Company;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Mail;

namespace LT.DigitalOffice.MessageService.Broker.Helpers
{
    public abstract class BaseEmailSender
    {
        private readonly IRequestClient<IGetSmtpCredentialsRequest> _rcGetSmtpCredentials;
        protected readonly ILogger _logger;

        private bool GetSmtpCredentials()
        {
            string logMessage = "Cannot get smtp credentials.";

            try
            {
                var result = _rcGetSmtpCredentials.GetResponse<IOperationResult<IGetSmtpCredentialsResponse>>(
                    IGetSmtpCredentialsRequest.CreateObj()).Result.Message;

                if (result.IsSuccess)
                {
                    SmtpCredentials.Host = result.Body.Host;
                    SmtpCredentials.Port = result.Body.Port;
                    SmtpCredentials.Email = result.Body.Email;
                    SmtpCredentials.Password = result.Body.Password;
                    SmtpCredentials.EnableSsl = result.Body.EnableSsl;

                    return true;
                }

                _logger?.LogWarning(logMessage);
            }
            catch(Exception exc)
            {
                _logger?.LogError(exc, logMessage);
            }

            return false;
        }

        protected bool Send(DbEmail email)
        {
            if (!SmtpCredentials.HasValue && !GetSmtpCredentials())
            {
                return false;
            }

            var message = new MailMessage(
                SmtpCredentials.Email,
                email.Receiver)
            {
                Subject = email.Subject,
                Body = email.Body
            };

            var smtp = new SmtpClient(
                SmtpCredentials.Host,
                SmtpCredentials.Port)
            {
                Credentials = new NetworkCredential(
                    SmtpCredentials.Email,
                    SmtpCredentials.Password),
                EnableSsl = SmtpCredentials.EnableSsl
            };

            try
            {
                smtp.Send(message);
            }
            catch (Exception exc)
            {
                _logger?.LogError(exc,
                    "Errors while sending email to {to} with subject: {subject} and body: {body}. Email replaced to resend queue.",
                    email.Receiver,
                    email.Subject,
                    email.Body);

                return false;
            }

            return true;
        }

        public BaseEmailSender(
            IRequestClient<IGetSmtpCredentialsRequest> rcGetSmtpCredentials,
            ILogger logger = null)
        {
            _rcGetSmtpCredentials = rcGetSmtpCredentials;
            _logger = logger;
        }
    }
}
