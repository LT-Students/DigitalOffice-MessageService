using LT.DigitalOffice.MessageService.Mappers.Patch.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.WorkspaceUser;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.MessageService.Mappers.Patch
{
  public class PatchDbWorkspaceUserMapper : IPatchDbWorkspaceUserMapper
  {
    public JsonPatchDocument<DbWorkspaceUser> Map(JsonPatchDocument<EditWorkspaceUserRequest> request)
    {
      if (request is null)
      {
        return null;
      }

      JsonPatchDocument<DbWorkspaceUser> result = new();

      foreach (var item in request.Operations)
      {
        result.Operations.Add(new(item.op, item.path, item.from, item.value));
      }

      return result;
    }
  }
}
