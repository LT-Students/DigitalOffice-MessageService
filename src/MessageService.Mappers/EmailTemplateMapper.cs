using LT.DigitalOffice.MessageService.Mappers.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto;
using System;

namespace LT.DigitalOffice.MessageService.Mappers
{
    public class EmailTemplateMapper : IMapper<EmailTemplate, DbEmailTemplate>,
        IMapper<EditEmailTemplateRequest, DbEmailTemplate>
    {
        public DbEmailTemplate Map(EmailTemplate emailTemplate)
        {
            if (emailTemplate == null)
            {
                throw new ArgumentNullException(nameof(emailTemplate));
            }

            return new DbEmailTemplate
            {
                Id = Guid.NewGuid(),
                Subject = emailTemplate.Subject,
                Body = emailTemplate.Body,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                AuthorId = emailTemplate.AuthorId
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
                Subject = emailTemplate.Subject,
                Body = emailTemplate.Body
            };
        }
    }
}
