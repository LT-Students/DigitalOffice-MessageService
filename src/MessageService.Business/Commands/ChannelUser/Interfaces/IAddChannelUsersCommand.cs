using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;

namespace LT.DigitalOffice.MessageService.Business.Commands.ChannelUser.Interfaces
{
  [AutoInject]
  public interface IAddChannelUsersCommand
  {
    Task<OperationResultResponse<bool>> ExecuteAsync(Guid channelId, List<Guid> usersIds);
  }
}
