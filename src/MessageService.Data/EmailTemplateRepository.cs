using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Data.Provider;
using LT.DigitalOffice.MessageService.Models.Db;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

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

        public DbEmailTemplate GetEmailTemplateById(Guid idEmailTemplate)
        {
            var dbEmailTemplate = provider.EmailTemplates
                .FirstOrDefault(et => et.Id == idEmailTemplate);

            if (dbEmailTemplate == null)
            {
                throw new NullReferenceException("Email template with this Id does not exist");
            }

            return dbEmailTemplate;
        }

        public void EditEmailTemplate(DbEmailTemplate dbEmailTemplateToEdit)
        {
            var dbTemplate = provider.EmailTemplates
                .AsNoTracking()
                .FirstOrDefault(et => et.Id == dbEmailTemplateToEdit.Id);

            if (dbTemplate == null)
            {
                throw new NullReferenceException("Email templates was not found.");
            }

            provider.EmailTemplates.Update(dbEmailTemplateToEdit);
            provider.Save();
        }
    }
}
