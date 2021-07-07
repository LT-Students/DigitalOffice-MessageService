using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;

namespace LT.DigitalOffice.MessageService.Broker.Helpers
{
    public class EmailSender : BaseEmailSender
    {
        private readonly IEmailRepository _emailRepository;
        private readonly IUnsentEmailRepository _unsentEmailRepository;

        public EmailSender(
            ILogger<EmailSender> logger,
            IRequestClient<IGetSmtpCredentialsRequest> rcGetSmtpCredentials,
            IEmailRepository emailRepository,
            IUnsentEmailRepository unsentEmailRepository)
            : base(rcGetSmtpCredentials, logger)
        {
            _emailRepository = emailRepository;
            _unsentEmailRepository = unsentEmailRepository;
        }

        public bool SendEmail(string to, string subject, string body)
        {
            DbEmail email = new()
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

        public bool ResendEmail(Guid unsentEmailId)
        {
            var unsentEmail = _unsentEmailRepository.Get(unsentEmailId);
            if (Send(unsentEmail.Email))
            {
                _unsentEmailRepository.Remove(unsentEmail);

                return true;
            }

            _unsentEmailRepository.IncrementTotalCount(unsentEmail);

            return false;
        }
    }
}