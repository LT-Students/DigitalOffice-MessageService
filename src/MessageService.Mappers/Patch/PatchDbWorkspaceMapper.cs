using System;
using System.Threading.Tasks;
using LT.DigitalOffice.ImageSupport.Helpers.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Patch.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Models.Image;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Newtonsoft.Json;

namespace LT.DigitalOffice.MessageService.Mappers.Patch
{
  public class PatchDbWorkspaceMapper : IPatchDbWorkspaceMapper
  {
    private readonly IImageResizeHelper _resizeHelper;

    public PatchDbWorkspaceMapper(IImageResizeHelper resizeHelper)
    {
      _resizeHelper = resizeHelper;
    }

    public async Task<JsonPatchDocument<DbWorkspace>> MapAsync(JsonPatchDocument<EditWorkspaceRequest> request)
    {
      if (request is null)
      {
        return null;
      }

      JsonPatchDocument<DbWorkspace> result = new();

      foreach (var item in request.Operations)
      {
        if (item.path.EndsWith(nameof(EditWorkspaceRequest.Image), StringComparison.OrdinalIgnoreCase))
        {
          ImageConsist image = JsonConvert.DeserializeObject<ImageConsist>(item.value.ToString());
          (bool _, string resizedContent, string extension) = await _resizeHelper.ResizeAsync(
            image.Content, image.Extension);
          result.Operations.Add(new Operation<DbWorkspace>(item.op, nameof(DbWorkspace.ImageContent), item.from, resizedContent));
          result.Operations.Add(new Operation<DbWorkspace>(item.op, nameof(DbWorkspace.ImageExtension), item.from, extension));

          continue;
        }

        result.Operations.Add(new Operation<DbWorkspace>(item.op, item.path, item.from, item.value));
      }

      return result;
    }
  }
}
