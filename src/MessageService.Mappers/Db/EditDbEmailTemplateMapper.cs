using LT.DigitalOffice.MessageService.Mappers.Db.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.EmailTemplate;
using System;

namespace LT.DigitalOffice.MessageService.Mappers.Db
{
    public class EditDbEmailTemplateMapper : IEditDbEmailTemplateMapper
    {
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
                Type = (int)emailTemplate.Type
            };
        }
    }
}
