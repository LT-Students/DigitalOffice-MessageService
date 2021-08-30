using LT.DigitalOffice.MessageService.Mappers.Models.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Models.Emails;

namespace LT.DigitalOffice.MessageService.Mappers.Models
{
  public class EmailTemplateTextInfoMapper : IEmailTemplateTextInfoMapper
  {
    public EmailTemplateTextInfo Map(DbEmailTemplateText templateText)
    {
      if (templateText == null)
      {
        return null;
      }

      return new EmailTemplateTextInfo
      {
        Id = templateText.Id,
        Subject = templateText.Subject,
        Text = templateText.Text,
        Language = templateText.Language
      };
    }
  }
}
