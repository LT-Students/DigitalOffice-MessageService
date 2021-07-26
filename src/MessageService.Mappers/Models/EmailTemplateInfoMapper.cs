using LT.DigitalOffice.MessageService.Mappers.Models.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Models.Emails;
using LT.DigitalOffice.Models.Broker.Enums;
using System;
using System.Linq;

namespace LT.DigitalOffice.MessageService.Mappers.Models
{
    public class EmailTemplateInfoMapper : IEmailTemplateInfoMapper
    {
        private readonly IEmailTemplateTextInfoMapper _mapper;

        public EmailTemplateInfoMapper(IEmailTemplateTextInfoMapper mapper)
        {
            _mapper = mapper;
        }

        public EmailTemplateInfo Map(DbEmailTemplate template)
        {
            if (template == null)
            {
                throw new ArgumentNullException(nameof(template));
            }

            return new EmailTemplateInfo
            {
                Id = template.Id,
                AuthorId = template.AuthorId,
                Name = template.Name,
                Type = ((EmailTemplateType)template.Type).ToString(),
                CreatedAt = template.CreatedAt,
                IsActive = template.IsActive,
                Texts = template.EmailTemplateTexts?.Select(_mapper.Map).ToList()
            };
        }
    }
}
