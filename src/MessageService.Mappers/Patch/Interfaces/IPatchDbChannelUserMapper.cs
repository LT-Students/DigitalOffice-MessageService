using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.ChannelUser;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.MessageService.Mappers.Patch.Interfaces
{
  [AutoInject]
  public interface IPatchDbChannelUserMapper
  {
    JsonPatchDocument<DbChannelUser> Map(JsonPatchDocument<EditChannelUserRequest> request);
  }
}
