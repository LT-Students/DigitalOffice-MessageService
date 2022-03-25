using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.WorkspaceUser;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.MessageService.Mappers.Patch.Interfaces
{
  [AutoInject]
  public interface IPatchDbWorkspaceUserMapper
  {
    JsonPatchDocument<DbWorkspaceUser> Map(JsonPatchDocument<EditWorkspaceUserRequest> request);
  }
}
