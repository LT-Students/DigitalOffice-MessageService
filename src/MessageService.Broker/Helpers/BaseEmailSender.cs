using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Mail;

namespace LT.DigitalOffice.MessageService.Broker.Helpers
{
    public abstract class BaseEmailSender
    {
        private readonly ISMTPCredentialRepository _repository;
        protected readonly ILogger _logger;

        protected bool Send(DbEmail email)
        {
            DbSMTPCredential smtpData = _repository.Get();

            var message = new MailMessage(
                smtpData.Email,
                email.Receiver)
            {
                Subject = email.Subject,
                Body = email.Body
            };

            var smtp = new SmtpClient(
                smtpData.Host,
                smtpData.Port)
            {
                Credentials = new NetworkCredential(
                                    smtpData.Email,
                                    smtpData.Password),
                EnableSsl = smtpData.EnableSsl
            };

            try
            {
                smtp.Send(message);
            }
            catch (Exception exc)
            {
                _logger?.LogWarning(exc,
                            "Errors while sending email to {to} with subject: {subject} and body: {body}. Email replaced to resend queue.",
                            email.Receiver,
                            email.Subject,
                            email.Body);

                return false;
            }

            return true;
        }

        public BaseEmailSender(
            ISMTPCredentialRepository repository,
            ILogger logger = null)
        {
            _repository = repository;
            _logger = logger;
        }
    }
}
