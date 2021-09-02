using System;
using LT.DigitalOffice.MessageService.Mappers.Db.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace LT.DigitalOffice.MessageService.Mappers.Db
{
  public class PatchDbWorkspaceMapper : IPatchDbWorkspaceMapper
  {
    public JsonPatchDocument<DbWorkspace> Map(JsonPatchDocument<EditWorkspaceRequest> request)
    {
      if (request == null)
      {
        return null;
      }

      JsonPatchDocument<DbWorkspace> dbPatch = new();

      foreach (var item in request.Operations)
      {
        dbPatch.Operations.Add(new Operation<DbWorkspace>(item.op, item.path, item.from, item.value));
      }

      return dbPatch;
    }
  }
}
