using System;
using LT.DigitalOffice.MessageService.Mappers.Patch.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace LT.DigitalOffice.MessageService.Mappers.Patch
{
  public class PatchDbWorkspaceMapper : IPatchDbWorkspaceMapper
  {
    public JsonPatchDocument<DbWorkspace> Map(JsonPatchDocument<EditWorkspaceRequest> request, Guid? imageId)
    {
      if (request == null)
      {
        return null;
      }

      JsonPatchDocument<DbWorkspace> response = new();

      foreach(var item in request.Operations)
      {
        if (item.path.EndsWith(nameof(EditWorkspaceRequest.Image), StringComparison.OrdinalIgnoreCase))
        {
          response.Operations.Add(new Operation<DbWorkspace>(item.op, nameof(DbWorkspace.ImageId), item.from, imageId));

          continue;
        }

        response.Operations.Add(new Operation<DbWorkspace>(item.op, item.path, item.from, item.value));
      }

      return response;
    }
  }
}
