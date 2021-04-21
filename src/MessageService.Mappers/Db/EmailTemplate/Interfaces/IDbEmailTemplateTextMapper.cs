using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Mappers.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Models;

namespace LT.DigitalOffice.MessageService.Mappers.Db.Interfaces
{
    [AutoInject]
    public interface IDbEmailTemplateTextMapper : IMapper<EmailTemplateTextInfo, DbEmailTemplateText>
    {
    }
}
