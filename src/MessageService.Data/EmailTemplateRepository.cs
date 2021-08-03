using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Data.Provider;
using LT.DigitalOffice.MessageService.Models.Db;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public bool Disable(Guid templateId)
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

            return true;
        }

        public Guid Add(DbEmailTemplate emailTemplate)
        {
            if (emailTemplate == null)
            {
                throw new ArgumentNullException(nameof(emailTemplate));
            }

            _provider.EmailTemplates.Add(emailTemplate);
            _provider.Save();

            return emailTemplate.Id;
        }

        public DbEmailTemplate Get(Guid id)
        {
            var dbEmailTemplate = _provider.EmailTemplates
                .Include(et => et.EmailTemplateTexts)
                .FirstOrDefault(et => et.Id == id);

            if (dbEmailTemplate == null)
            {
                throw new NotFoundException($"Email template with this ID '{id}' does not exist");
            }

            return dbEmailTemplate;
        }

        public DbEmailTemplate Get(int type)
        {
            var dbEmailTemplate = _provider.EmailTemplates.Include(et => et.EmailTemplateTexts)
                .FirstOrDefault(et => et.Type == type && et.IsActive);

            if (dbEmailTemplate == null)
            {
                throw new NotFoundException($"Email template with this type '{type}' does not exist");
            }

            return dbEmailTemplate;
        }

        public bool Edit(DbEmailTemplate dbEmailTemplateToEdit)
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

            return true;
        }

        public List<DbEmailTemplate> Find(int skipCount, int takeCount, bool? includeDeactivated, out int totalCount)
        {
            if (skipCount < 0)
            {
                throw new BadRequestException("Skip count can't be less than 0.");
            }

            if (takeCount <= 0)
            {
                throw new BadRequestException("Take count can't be equal or less than 0.");
            }

            var dbEmails = _provider.EmailTemplates.AsQueryable();

            if (includeDeactivated.HasValue && includeDeactivated.Value)
            {
                totalCount = _provider.EmailTemplates.Count();
            }
            else
            {
                dbEmails = dbEmails.Where(e => e.IsActive);
                totalCount = _provider.EmailTemplates.Count(e => e.IsActive);
            }

            return dbEmails.Skip(skipCount).Take(takeCount).Include(e => e.EmailTemplateTexts).ToList();
        }
    }
}
