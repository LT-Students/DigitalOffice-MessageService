using LT.DigitalOffice.MessageService.Mappers.Patch.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.ChannelUser;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.MessageService.Mappers.Patch
{
  public class PatchDbChannelUserMapper : IPatchDbChannelUserMapper
  {
    public JsonPatchDocument<DbChannelUser> Map(JsonPatchDocument<EditChannelUserRequest> request)
    {
      if (request is null)
      {
        return null;
      }

      JsonPatchDocument<DbChannelUser> result = new();

      foreach (var item in request.Operations)
      {
        result.Operations.Add(new(item.op, item.path, item.from, item.value));
      }

      return result;
    }
  }
}
