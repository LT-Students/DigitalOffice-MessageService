using LT.DigitalOffice.Kernel.Exceptions;
using LT.DigitalOffice.Kernel.Exceptions.Models;
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

        public void DisableEmailTemplate(Guid templateId)
        {
            var dbEmailTemplateText = provider.EmailTemplates
                .FirstOrDefault(templateText => templateText.Id == templateId);

            if (dbEmailTemplateText == null)
            {
                throw new NotFoundException($"Email template with this ID '{templateId}' does not exist.");
            }

            dbEmailTemplateText.IsActive = false;

            provider.EmailTemplates.Update(dbEmailTemplateText);
            provider.Save();
        }

        public Guid AddEmailTemplate(DbEmailTemplate emailTemplate)
        {
            if (emailTemplate == null)
            {
                throw new ArgumentNullException(nameof(emailTemplate));
            }

            provider.EmailTemplates.Add(emailTemplate);
            provider.Save();

            return emailTemplate.Id;
        }

        public DbEmailTemplate GetEmailTemplateById(Guid emailTemplateId)
        {
            var dbEmailTemplate = provider.EmailTemplates
                .FirstOrDefault(et => et.Id == emailTemplateId);

            if (dbEmailTemplate == null)
            {
                throw new NotFoundException($"Email template with this ID '{emailTemplateId}' does not exist");
            }

            return dbEmailTemplate;
        }

        public DbEmailTemplate GetEmailTemplateByType(int type)
        {
            var dbEmailTemplate = provider.EmailTemplates
                .FirstOrDefault(et => et.Type == type);

            if (dbEmailTemplate == null)
            {
                throw new NotFoundException($"Email template with this type '{type}' does not exist");
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
                throw new NotFoundException("Email template with this ID '{templateId}' does not exist.");
            }

            provider.EmailTemplates.Update(dbEmailTemplateToEdit);
            provider.Save();
        }
    }
}
