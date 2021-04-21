using LT.DigitalOffice.MessageService.Mappers.Db.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Models;
using System;

namespace LT.DigitalOffice.MessageService.Mappers.Db
{
    public class DbEmailTemplateTextMapper : IDbEmailTemplateTextMapper
    {
        public DbEmailTemplateText Map(EmailTemplateTextInfo templateText)
        {
            if (templateText == null)
            {
                throw new ArgumentNullException(nameof(templateText));
            }

            return new DbEmailTemplateText
            {
                Subject = templateText.Subject,
                Text = templateText.Text,
                Language = templateText.Language
            };
        }
    }
}
