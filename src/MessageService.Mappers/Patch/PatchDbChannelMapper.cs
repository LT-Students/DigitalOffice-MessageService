using System;
using System.Threading.Tasks;
using LT.DigitalOffice.MessageService.Mappers.Patch.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Channel;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace LT.DigitalOffice.MessageService.Mappers.Patch
{
  public class PatchDbChannelMapper : IPatchDbChannelMapper
  {
    public JsonPatchDocument<DbChannel> Map(JsonPatchDocument<EditChannelRequest> request)
    {
      if (request is null)
      {
        return null;
      }

      JsonPatchDocument<DbChannel> result = new();

      foreach (var item in request.Operations)
      {
        result.Operations.Add(new Operation<DbChannel>(item.op, item.path, item.from, item.value));
      }

      return result;
    }
  }
}
