using LT.DigitalOffice.MessageService.Models.Db;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Mail;

namespace LT.DigitalOffice.MessageService.Broker.Helpers
{
    public abstract class BaseEmailSender
    {
        protected readonly SmtpCredentialsOptions _options;
        protected readonly ILogger _logger;

        protected bool Send(DbEmail email)
        {
            var message = new MailMessage(_options.Email, email.Receiver)
            {
                Subject = email.Subject,
                Body = email.Body
            };

            var smtp = new SmtpClient(_options.Host, _options.Port)
            {
                Credentials = new NetworkCredential(_options.Email, _options.Password),
                EnableSsl = true
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
            SmtpCredentialsOptions options, 
            ILogger logger = null)
        {
            _options = options;
            _logger = logger;
        }
    }
}
