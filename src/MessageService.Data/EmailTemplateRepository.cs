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

        public Guid AddEmailTemplate(DbEmailTemplate emailTemplate)
        {
            provider.EmailTemplates.Add(emailTemplate);
            provider.Save();

            return emailTemplate.Id;
        }
    }
}
