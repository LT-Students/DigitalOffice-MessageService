using LT.DigitalOffice.MessageService.Mappers.Db.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.EmailTemplate;
using System;
using System.Linq;

namespace LT.DigitalOffice.MessageService.Mappers.Db
{
    public class DbEmailTemplateMapper : IDbEmailTemplateMapper
    {
        public DbEmailTemplate Map(EmailTemplateRequest emailTemplate)
        {
            if (emailTemplate == null)
            {
                throw new ArgumentNullException(nameof(emailTemplate));
            }

            var templateId = Guid.NewGuid();

            return new DbEmailTemplate
            {
                Id = templateId,
                Name = emailTemplate.Name,
                Type = (int)emailTemplate.Type,
                CreatedAtUtc = DateTime.UtcNow,
                IsActive = true,
                CreatedBy = emailTemplate.AuthorId,
                EmailTemplateTexts = emailTemplate.EmailTemplateTexts.Select(x =>
                    new DbEmailTemplateText
                    {
                        Id = Guid.NewGuid(),
                        EmailTemplateId = templateId,
                        Subject = x.Subject,
                        Text = x.Text,
                        Language = x.Language
                    }
                )
                .ToList()
            };
        }
    }
}
