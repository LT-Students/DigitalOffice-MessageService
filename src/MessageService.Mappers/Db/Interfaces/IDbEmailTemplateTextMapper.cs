using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Models.Emails;

namespace LT.DigitalOffice.MessageService.Mappers.Db.Interfaces
{
    [AutoInject]
    public interface IDbEmailTemplateTextMapper
    {
        DbEmailTemplateText Map(EmailTemplateTextInfo emailTemplateTextInfo);
    }
}
