using System;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Models.Dto.Requests;

namespace LT.DigitalOffice.MessageService.Business.Commands.Channels.Interfaces
{
  [AutoInject]
  public interface ICreateChannelCommand
  {
    OperationResultResponse<Guid?> Exeсute(CreateChannelRequest request);
  }
}
