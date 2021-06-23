using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Helpers;
using LT.DigitalOffice.MessageService.Models.Db;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Mail;

namespace LT.DigitalOffice.MessageService.Broker.Helpers
{
    public abstract class BaseEmailSender
    {
        protected readonly ILogger _logger;

        protected bool Send(DbEmail email)
        {
            string senderEmail = GetEnvironmentVariableHelper.Get(ConstStrings.Email);

            var message = new MailMessage(
                senderEmail,
                email.Receiver)
            {
                Subject = email.Subject,
                Body = email.Body
            };

            var smtp = new SmtpClient(
                GetEnvironmentVariableHelper.Get(ConstStrings.Host),
                int.Parse(GetEnvironmentVariableHelper.Get(ConstStrings.Port)))
            {
                Credentials = new NetworkCredential(
                                    senderEmail,
                                    GetEnvironmentVariableHelper.Get(ConstStrings.Password)),
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
            ILogger logger = null)
        {
            _logger = logger;
        }
    }
}
