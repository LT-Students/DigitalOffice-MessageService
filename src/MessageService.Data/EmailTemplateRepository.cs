using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Data.Provider;
using LT.DigitalOffice.MessageService.Models.Db;
using System;

namespace LT.DigitalOffice.MessageService.Data
{
    public class EmailTemplateRepository : IEmailTemplateRepository
    {
        private readonly IDataProvider provider;

        public EmailTemplateRepository(IDataProvider provider)
        {
            this.provider = provider;
        }

        public void DisableEmailTemplate(Guid emailTemplateId)
        {
            var dbEmailTemplate = provider.EmailTemplates.Find(emailTemplateId);

            if (dbEmailTemplate == null)
            {
                throw new Exception("Email template with this Id does not exist.");
            }

            dbEmailTemplate.IsActive = false;

            provider.EmailTemplates.Update(dbEmailTemplate);
            provider.Save();
        }

        public Guid AddEmailTemplate(DbEmailTemplate emailTemplate)
        {
            provider.EmailTemplates.Add(emailTemplate);
            provider.Save();

            return emailTemplate.Id;
        }
    }
}
