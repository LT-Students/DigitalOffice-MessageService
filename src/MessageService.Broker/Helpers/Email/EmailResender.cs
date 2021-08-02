using LT.DigitalOffice.MessageService.Data;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Data.Provider;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace LT.DigitalOffice.MessageService.Broker.Helpers
{
    public class EmailResender : BaseEmailSender
    {
        private readonly IUnsentEmailRepository _unsentEmailRepository;

        public void StartResend(double intervalInMinutes)
        {
            while (true)
            {
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

        public EmailResender(
            IUnsentEmailRepository unsentEmailRepository,
            IRequestClient<IGetSmtpCredentialsRequest> rcGetSmtpCredentials)
            : base(rcGetSmtpCredentials, null)
        {
            _unsentEmailRepository = unsentEmailRepository;
        }
    }
}
