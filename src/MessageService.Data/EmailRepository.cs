using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Data.Provider;
using LT.DigitalOffice.MessageService.Models.Db;

namespace LT.DigitalOffice.MessageService.Data
{
    public class EmailRepository : IEmailRepository
    {
        private readonly IDataProvider _provider;

        public EmailRepository(IDataProvider provider)
        {
            _provider = provider;
        }

        public void SaveEmail(DbEmail email)
        {
            _provider.Emails.Add(email);
            _provider.Save();
        }
    }
}