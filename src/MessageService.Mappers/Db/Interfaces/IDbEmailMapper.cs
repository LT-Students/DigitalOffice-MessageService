using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Db;

namespace LT.DigitalOffice.MessageService.Mappers.Db.Email.Interfaces
{
    [AutoInject]
    public interface IDbEmailMapper
    {
        DbEmail Map(ISendEmailRequest request);
    }
}
