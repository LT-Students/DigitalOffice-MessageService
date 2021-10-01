using System;
using LT.DigitalOffice.MessageService.Mappers.Db.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.EmailTemplate;

namespace LT.DigitalOffice.MessageService.Mappers.Db
{
  public class DbEmailTemplateTextMapper : IDbEmailTemplateTextMapper
  {
    public DbEmailTemplateText Map(EmailTemplateTextRequest request)
    {
      if (request == null)
      {
        return null;
      }

      return new DbEmailTemplateText
      {
        Id = Guid.NewGuid(),
        EmailTemplateId = request.EmailTemplateId.Value,
        Subject = request.Subject,
        Text = request.Text,
        Language = request.Language
      };
    }
  }
}
