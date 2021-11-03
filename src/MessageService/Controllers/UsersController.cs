using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Business.Commands.User.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.MessageService.Controllers
{
  [ApiController]
  [Route("[Controller]")]
  public class UsersController : ControllerBase
  {
    [HttpPost("createworkspaceuser")]
    public async Task<OperationResultResponse<bool>> CreateAsync(
      [FromServices] ICreateWorkspaceUserCommand command,
      [FromQuery] Guid workspaceId,
      [FromBody] List<Guid> usersIds)
    {
      return await command.ExecuteAsync(workspaceId, usersIds);
    }

    [HttpDelete("removeworkspaceuser")]
    public async Task<OperationResultResponse<bool>> RemoveAsync(
      [FromServices] IRemoveWorkspaceUserCommand command,
      [FromQuery] Guid workspaceId,
      [FromBody] List<Guid> usersIds)
    {
      return await command.ExecuteAsync(workspaceId, usersIds);
    }

    [HttpPost("createcahnneluser")]
    public async Task<OperationResultResponse<bool>> CreateAsync(
      [FromServices] ICreateChannelUserCommand command,
      [FromQuery] Guid channelId,
      [FromBody] List<Guid> usersIds)
    {
      return await command.ExecuteAsync(channelId, usersIds);
    }

    [HttpDelete("removecahnneluser")]
    public async Task<OperationResultResponse<bool>> RemoveAsync(
      [FromServices] IRemoveChannelUserCommand command,
      [FromQuery] Guid channelId,
      [FromBody] List<Guid> usersIds)
    {
      return await command.ExecuteAsync(channelId, usersIds);
    }
  }
}
