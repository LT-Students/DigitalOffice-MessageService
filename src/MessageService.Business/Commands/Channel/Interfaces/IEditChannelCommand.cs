using System;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Channel;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.MessageService.Business.Commands.Channel.Interfaces
{
  [AutoInject]
  public interface IEditChannelCommand
  {
    Task<OperationResultResponse<bool>> ExecuteAsync(
      Guid channelId, JsonPatchDocument<EditChannelRequest> document);
  }
}
