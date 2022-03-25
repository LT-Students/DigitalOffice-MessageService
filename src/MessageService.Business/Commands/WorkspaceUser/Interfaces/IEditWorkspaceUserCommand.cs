using System;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.WorkspaceUser;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.MessageService.Business.Commands.WorkspaceUser.Interfaces
{
  [AutoInject]
  public interface IEditWorkspaceUserCommand
  {
    Task<OperationResultResponse<bool>> ExecuteAsync(
      Guid channelId, Guid userId, JsonPatchDocument<EditWorkspaceUserRequest> document);
  }
}
