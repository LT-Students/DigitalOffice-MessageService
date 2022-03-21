using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Responses;
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
      [FromRoute] Guid workspaceId,
      [FromBody] List<Guid> usersIds)
    {
      throw new NotImplementedException();
    }

    [HttpDelete]
    public async Task<OperationResultResponse<bool>> DeleteUsersAsync(
      [FromRoute] Guid workspaceId,
      [FromBody] List<Guid> usersIds)
    {
      throw new NotImplementedException();
    }

    [HttpPatch("{userId}")]
    public async Task<OperationResultResponse<bool>> EditUserAsync(
      [FromRoute] Guid workspaceId,
      [FromRoute] Guid userId,
      [FromBody] JsonPatchDocument<EditWorkspaceUserRequest> request)
    {
      throw new NotImplementedException();
    }
  }
}
