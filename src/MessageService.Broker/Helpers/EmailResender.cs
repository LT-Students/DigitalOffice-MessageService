using LT.DigitalOffice.MessageService.Data;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Data.Provider;
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

        public EmailResender(IDataProvider dataProvider)
            : base(new SMTPCredentialRepository(dataProvider))
        {
            _unsentEmailRepository = new UnsentEmailRepository(dataProvider);
        }
    }
}
