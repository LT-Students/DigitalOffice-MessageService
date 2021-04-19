using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Mappers.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.EmailTemplate;

namespace LT.DigitalOffice.MessageService.Mappers.Db.Interfaces
{
    [AutoInject]
    public interface IDbEmailTemplateMapper : IMapper<EmailTemplateRequest, DbEmailTemplate>
    {
    }
}
