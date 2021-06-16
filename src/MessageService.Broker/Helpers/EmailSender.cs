using LT.DigitalOffice.MessageService;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace LT.DigitalOffice.UserService.Business.Helpers.Email
{
    public class EmailSender
    {
        private const int ResendInterval = 15;

        private readonly IEmailRepository _emailRepository;
        private readonly IUnsentEmailRepository _unsentEmailRepository;
        private static ILogger<EmailSender> _logger;
        private readonly IOptions<SmtpCredentialsOptions> _options;

        private bool Send(DbEmail email)
        {
            var message = new MailMessage(_options.Value.Email, email.Receiver)
            {
                Subject = email.Subject,
                Body = email.Body
            };

            var smtp = new SmtpClient(_options.Value.Host, _options.Value.Port)
            {
                Credentials = new NetworkCredential(_options.Value.Email, _options.Value.Password),
                EnableSsl = true
            };

            try
            {
                smtp.Send(message);
            }
            catch (Exception exc)
            {
                _logger.LogWarning(exc,
                            "Errors while sending email to {to} with subject: {subject} and body: {body}. Email is in resend queue.",
                            email.Receiver,
                            email.Subject,
                            email.Body);

                return false;
            }

            return true;
        }

        private void StartResend(double intervalInMinutes)
        {
            while (true)
            {
                Task.Delay(TimeSpan.FromMinutes(intervalInMinutes)).Wait();

                var unsentEmails = _unsentEmailRepository.GetAll();

                foreach (var email in unsentEmails)
                {
                    var isSuccess = Send(email.Email);

                    if (isSuccess)
                    {
                        _unsentEmailRepository.Remove(email);
                    }
                    else
                    {
                        _unsentEmailRepository.IncrementTotalCount(email);
                    }       
                }
            }
        }

        public EmailSender(
            ILogger<EmailSender> logger,
            IOptions<SmtpCredentialsOptions> options,
            IEmailRepository emailRepository,
            IUnsentEmailRepository unsentEmailRepository)
        {
            _logger = logger;
            _options = options;
            _emailRepository = emailRepository;
            _unsentEmailRepository = unsentEmailRepository;
            StartResend(ResendInterval);
        }

        public bool SendEmail(string to, string subject, string body)
        {
            DbEmail email = new DbEmail
            {
                Id = Guid.NewGuid(),
                Body = body,
                Subject = subject,
                Receiver = to,
                Time = DateTime.UtcNow
            };

            _emailRepository.SaveEmail(email);

            if (Send(email)) 
            {
                return true;
            }

            _unsentEmailRepository.Add(
                new DbUnsentEmail
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = email.Time,
                    LastSendAt = email.Time,
                    EmailId = email.Id,
                    TotalSendingCount = 1
                });

            return false;
        }
    }
}