using LT.DigitalOffice.MessageService.Mappers.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto;
using LT.DigitalOffice.MessageService.Models.Dto.Requests;
using System;
using System.Linq;

namespace LT.DigitalOffice.MessageService.Mappers.EmailMappers
{
    public class EmailTemplateMapper : IMapper<EmailTemplateRequest, DbEmailTemplate>,
        IMapper<EditEmailTemplateRequest, DbEmailTemplate>
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
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                AuthorId = emailTemplate.AuthorId,
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

        public DbEmailTemplate Map(EditEmailTemplateRequest emailTemplate)
        {
            if (emailTemplate == null)
            {
                throw new ArgumentNullException(nameof(emailTemplate));
            }

            return new DbEmailTemplate
            {
                Id = emailTemplate.Id,
                Name = emailTemplate.Name,
                Type = (int)emailTemplate.Type,
                EmailTemplateTexts = emailTemplate.EmailTemplateTexts.Select(x =>
                    new DbEmailTemplateText
                    {
                        Id = Guid.NewGuid(),
                        EmailTemplateId = emailTemplate.Id,
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
