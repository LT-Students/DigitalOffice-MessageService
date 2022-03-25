using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Channel;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.MessageService.Mappers.Patch.Interfaces
{
  [AutoInject]
  public interface IPatchDbChannelMapper
  {
    JsonPatchDocument<DbChannel> Map(JsonPatchDocument<EditChannelRequest> request);
  }
}
