using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Data.Provider;
using LT.DigitalOffice.MessageService.Models.Db;

namespace LT.DigitalOffice.MessageService.Data
{
    public class EmailRepository : IEmailRepository
    {
        private readonly IDataProvider provider;

        public EmailRepository(IDataProvider provider)
        {
            this.provider = provider;
        }

        public void SaveEmail(DbEmail email)
        {
            provider.Emails.Add(email);
            provider.Save();
        }
    }
}