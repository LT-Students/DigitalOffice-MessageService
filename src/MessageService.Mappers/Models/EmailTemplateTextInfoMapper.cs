using LT.DigitalOffice.MessageService.Mappers.Models.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Models.Emails;
using System;

namespace LT.DigitalOffice.MessageService.Mappers.Models
{
    public class EmailTemplateTextInfoMapper : IEmailTemplateTextInfoMapper
    {
        public EmailTemplateTextInfo Map(DbEmailTemplateText templateText)
        {
            if (templateText == null)
            {
                throw new ArgumentNullException(nameof(templateText));
            }

            return new EmailTemplateTextInfo
            {
                Id = templateText.Id,
                Language = templateText.Language,
                Subject = templateText.Subject,
                Text = templateText.Text
            };
        }
    }
}
