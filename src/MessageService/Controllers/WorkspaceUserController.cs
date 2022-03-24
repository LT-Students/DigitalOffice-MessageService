using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Business.Commands.WorkspaceUser.Interfaces;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.WorkspaceUser;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.MessageService.Controllers
{
  [Route("workspace/{workspaceId}/user")]
  public class WorkspaceUserController : ControllerBase
  {
    [HttpPost]
    public async Task<OperationResultResponse<bool>> AddUsersAsync(
      [FromServices] IAddWorkspaceUsersCommand command,
      [FromRoute] Guid workspaceId,
      [FromBody] List<Guid> usersIds)
    {
      return await command.ExecuteAsync(workspaceId, usersIds);
    }

    [HttpDelete]
    public async Task<OperationResultResponse<bool>> DeleteUsersAsync(
      [FromServices] IDeleteWorkspaceUsersCommand command,
      [FromRoute] Guid workspaceId,
      [FromBody] List<Guid> usersIds)
    {
      return await command.ExecuteAsync(workspaceId, usersIds);
    }

    [HttpPatch("{userId}")]
    public async Task<OperationResultResponse<bool>> EditUserAsync(
      [FromServices] IEditWorkspaceUserCommand command,
      [FromRoute] Guid workspaceId,
      [FromRoute] Guid userId,
      [FromBody] JsonPatchDocument<EditWorkspaceUserRequest> request)
    {
      return await command.ExecuteAsync(workspaceId, userId, request);
    }
  }
}
