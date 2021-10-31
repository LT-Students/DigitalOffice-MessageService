using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.MessageService.Mappers.Patch.Interfaces
{
  [AutoInject]
  public interface IPatchDbWorkspaceMapper
  {
    Task<JsonPatchDocument<DbWorkspace>> MapAsync(JsonPatchDocument<EditWorkspaceRequest> request);
  }
}
