using LT.DigitalOffice.MessageService.Data;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Data.Provider;
using LT.DigitalOffice.MessageService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.MessageService.Models.Db;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace LT.DigitalOffice.MessageService.Broker.Helpers
{
    public class EmailResender
    {
        private IUnsentEmailRepository _unsentEmailRepository;
        private SmtpCredentialsOptions _credentialsOptions;

        private bool Send(DbEmail email)
        {
            var message = new MailMessage(_credentialsOptions.Email, email.Receiver)
            {
                Subject = email.Subject,
                Body = email.Body
            };

            var smtp = new SmtpClient(_credentialsOptions.Host, _credentialsOptions.Port)
            {
                Credentials = new NetworkCredential(_credentialsOptions.Email, _credentialsOptions.Password),
                EnableSsl = true
            };

            try
            {
                smtp.Send(message);
            }
            catch
            {
                return false;
            }

            return true;
        }

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
            SmtpCredentialsOptions credentialsOptions,
            string sqlConnectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MessageServiceDbContext>();
            optionsBuilder.UseSqlServer(sqlConnectionString);
            IDataProvider dataProvider = new MessageServiceDbContext(optionsBuilder.Options);
            _unsentEmailRepository = new UnsentEmailRepository(dataProvider);

            _credentialsOptions = credentialsOptions;
        }
    }
}
