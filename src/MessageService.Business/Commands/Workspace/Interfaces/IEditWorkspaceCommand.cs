using System;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.MessageService.Business.Commands.Workspace.Interfaces
{
  [AutoInject]
  public interface IEditWorkspaceCommand
  {
    Task<OperationResultResponse<bool>> ExecuteAsync(Guid workspaceId, JsonPatchDocument<EditWorkspaceRequest> request);
  }
}
