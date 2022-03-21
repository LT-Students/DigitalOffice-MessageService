using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Responses;
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
      [FromRoute] Guid channelId,
      [FromBody] List<Guid> usersIds)
    {
      throw new NotImplementedException();
    }

    [HttpDelete]
    public async Task<OperationResultResponse<bool>> DeleteUsersAsync(
      [FromRoute] Guid channelId,
      [FromBody] List<Guid> usersIds)
    {
      throw new NotImplementedException();
    }

    [HttpPatch("{userId}")]
    public async Task<OperationResultResponse<bool>> EditUserAsync(
      [FromRoute] Guid channelId,
      [FromRoute] Guid userId,
      [FromBody] JsonPatchDocument<EditChannelUserRequest> request)
    {
      throw new NotImplementedException();
    }
  }
}
