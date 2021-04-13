using LT.DigitalOffice.MessageService.Mappers.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Models;
using System;
namespace LT.DigitalOffice.MessageService.Mappers.EmailMappers
{
    public class EmailTemplateTextMapper : IMapper<EmailTemplateTextInfo, DbEmailTemplateText>
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
