using System;
using System.Linq;
using LT.DigitalOffice.MessageService.Mappers.Models.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Models.Emails;
using LT.DigitalOffice.Models.Broker.Enums;

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
        return null;
      }

      return new EmailTemplateInfo
      {
        Id = template.Id,
        Name = template.Name,
        Type = ((EmailTemplateType)template.Type).ToString(),
        IsActive = template.IsActive,
        CreatedBy = template.CreatedBy,
        CreatedAtUtc = template.CreatedAtUtc,
        Texts = template.EmailTemplateTexts?.Select(_mapper.Map).ToList()
      };
    }
  }
}
