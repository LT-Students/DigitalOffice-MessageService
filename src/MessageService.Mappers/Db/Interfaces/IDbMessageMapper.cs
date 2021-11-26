using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Message;

namespace LT.DigitalOffice.MessageService.Mappers.Db.Interfaces
{
  [AutoInject]
  public interface IDbMessageMapper
  {
    DbMessage Map(CreateMessageRequest request);
  }
}
