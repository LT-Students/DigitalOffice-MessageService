using System;
using LT.DigitalOffice.MessageService.Mappers.Helpers.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Patch.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Models.Workspace;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Newtonsoft.Json;

namespace LT.DigitalOffice.MessageService.Mappers.Patch
{
  public class PatchDbWorkspaceMapper : IPatchDbWorkspaceMapper
  {
    private readonly IResizeImageHelper _resizeHelper;

    public PatchDbWorkspaceMapper(IResizeImageHelper resizeHelper)
    {
      _resizeHelper = resizeHelper;
    }

    public JsonPatchDocument<DbWorkspace> Map(JsonPatchDocument<EditWorkspaceRequest> request)
    {
      if (request == null)
      {
        return null;
      }

      JsonPatchDocument<DbWorkspace> result = new();

      foreach(var item in request.Operations)
      {
        if (item.path.EndsWith(nameof(EditWorkspaceRequest.Avatar), StringComparison.OrdinalIgnoreCase))
        {
          AvatarData avatar = JsonConvert.DeserializeObject <AvatarData>(item.value.ToString());
          var resizedContent = _resizeHelper.Resize(avatar.Content, avatar.Extension);
          result.Operations.Add(new Operation<DbWorkspace>(item.op, nameof(DbWorkspace.AvatarContent), item.from, resizedContent));
          result.Operations.Add(new Operation<DbWorkspace>(item.op, nameof(DbWorkspace.AvatarExtension), item.from, avatar.Extension));

          continue;
        }

        result.Operations.Add(new Operation<DbWorkspace>(item.op, item.path, item.from, item.value));
      }

      return result;
    }
  }
}
