using LT.DigitalOffice.MessageService.Data;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Data.Provider;
using LT.DigitalOffice.MessageService.Mappers.Models.Interfaces;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using MassTransit;
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

        public EmailResender(
            IDataProvider dataProvider,
            IRequestClient<IGetSmtpCredentialsRequest> rcGetSmtpCredentials,
            ISmtpCredentialsMapper mapper)
            : base(rcGetSmtpCredentials, mapper)
        {
            _unsentEmailRepository = new UnsentEmailRepository(dataProvider);
        }
    }
}
