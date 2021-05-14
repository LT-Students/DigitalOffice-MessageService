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
        private readonly IDataProvider _provider;

        public EmailTemplateRepository(IDataProvider provider)
        {
            _provider = provider;
        }

        public void DisableEmailTemplate(Guid templateId)
        {
            var dbEmailTemplateText = _provider.EmailTemplates
                .FirstOrDefault(templateText => templateText.Id == templateId);

            if (dbEmailTemplateText == null)
            {
                throw new NotFoundException($"Email template with this ID '{templateId}' does not exist.");
            }

            dbEmailTemplateText.IsActive = false;

            _provider.EmailTemplates.Update(dbEmailTemplateText);
            _provider.Save();
        }

        public Guid AddEmailTemplate(DbEmailTemplate emailTemplate)
        {
            if (emailTemplate == null)
            {
                throw new ArgumentNullException(nameof(emailTemplate));
            }

            _provider.EmailTemplates.Add(emailTemplate);
            _provider.Save();

            return emailTemplate.Id;
        }

        public DbEmailTemplate GetEmailTemplateById(Guid emailTemplateId)
        {
            var dbEmailTemplate = _provider.EmailTemplates
                .FirstOrDefault(et => et.Id == emailTemplateId);

            if (dbEmailTemplate == null)
            {
                throw new NotFoundException($"Email template with this ID '{emailTemplateId}' does not exist");
            }

            return dbEmailTemplate;
        }

        public DbEmailTemplate GetEmailTemplateByType(int type)
        {
            var dbEmailTemplate = _provider.EmailTemplates
                .FirstOrDefault(et => et.Type == type);

            if (dbEmailTemplate == null)
            {
                throw new NotFoundException($"Email template with this type '{type}' does not exist");
            }

            return dbEmailTemplate;
        }

        public void EditEmailTemplate(DbEmailTemplate dbEmailTemplateToEdit)
        {
            var dbTemplate = _provider.EmailTemplates
                .AsNoTracking()
                .FirstOrDefault(et => et.Id == dbEmailTemplateToEdit.Id);

            if (dbTemplate == null)
            {
                throw new NotFoundException($"Email template with this ID '{dbEmailTemplateToEdit.Id}' does not exist.");
            }

            _provider.EmailTemplates.Update(dbEmailTemplateToEdit);
            _provider.Save();
        }
    }
}
