using System;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.ChannelUser;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.MessageService.Business.Commands.ChannelUser.Interfaces
{
  [AutoInject]
  public interface IEditChannelUserCommand
  {
    Task<OperationResultResponse<bool>> ExecuteAsync(
      Guid channelId, Guid userId, JsonPatchDocument<EditChannelUserRequest> document);
  }
}
