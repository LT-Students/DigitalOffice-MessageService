using LT.DigitalOffice.MessageService.Data;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Data.Provider;
using LT.DigitalOffice.MessageService.Data.Provider.MsSql.Ef;
using Microsoft.EntityFrameworkCore;
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
            string sqlConnectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MessageServiceDbContext>();
            optionsBuilder.UseSqlServer(sqlConnectionString);
            IDataProvider dataProvider = new MessageServiceDbContext(optionsBuilder.Options);
            _unsentEmailRepository = new UnsentEmailRepository(dataProvider);
        }
    }
}
