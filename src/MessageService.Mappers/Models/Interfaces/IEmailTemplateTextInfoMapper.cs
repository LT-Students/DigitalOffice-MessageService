using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Models.Emails;

namespace LT.DigitalOffice.MessageService.Mappers.Models.Interfaces
{
    [AutoInject]
    public interface IEmailTemplateTextInfoMapper
    {
        EmailTemplateTextInfo Map(DbEmailTemplateText templateText);
    }
}
