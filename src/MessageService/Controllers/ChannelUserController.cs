using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Business.Commands.ChannelUser.Interfaces;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.ChannelUser;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.MessageService.Controllers
{
  [Route("channel/{channelId}/user")]
  public class ChannelUserController : ControllerBase
  {
    [HttpPost]
    public async Task<OperationResultResponse<bool>> AddUsersAsync(
      [FromServices] IAddChannelUsersCommand command,
      [FromRoute] Guid channelId,
      [FromBody] List<Guid> usersIds)
    {
      return await command.ExecuteAsync(channelId, usersIds);
    }

    [HttpDelete]
    public async Task<OperationResultResponse<bool>> DeleteUsersAsync(
      [FromServices] IDeleteChannelUsersCommand command,
      [FromRoute] Guid channelId,
      [FromBody] List<Guid> usersIds)
    {
      return await command.ExecuteAsync(channelId, usersIds);
    }

    [HttpPatch("{userId}")]
    public async Task<OperationResultResponse<bool>> EditUserAsync(
      [FromServices] IEditChannelUserCommand command,
      [FromRoute] Guid channelId,
      [FromRoute] Guid userId,
      [FromBody] JsonPatchDocument<EditChannelUserRequest> request)
    {
      return await command.ExecuteAsync(channelId, userId, request);
    }
  }
}
