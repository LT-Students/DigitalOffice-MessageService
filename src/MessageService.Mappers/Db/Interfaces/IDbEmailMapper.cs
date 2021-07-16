using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.Models.Broker.Requests.Message;

namespace LT.DigitalOffice.MessageService.Mappers.Db.Email.Interfaces
{
    [AutoInject]
    public interface IDbEmailMapper
    {
        DbEmail Map(ISendEmailRequest request);
    }
}
