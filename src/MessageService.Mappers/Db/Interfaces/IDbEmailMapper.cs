using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Mappers.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.Models.Broker.Requests.Message;

namespace LT.DigitalOffice.MessageService.Mappers.Db.Interfaces
{
    [AutoInject]
    public interface IDbEmailMapper : IMapper<ISendEmailRequest, DbEmail>
    {
    }
}
