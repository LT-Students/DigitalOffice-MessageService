using System;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Models.Dto.Requests;

namespace LT.DigitalOffice.MessageService.Business.Commands.Channel.Interfaces
{
  [AutoInject]
  public interface ICreateChannelCommand
  {
    Task<OperationResultResponse<Guid?>> ExeсuteAsync(CreateChannelRequest request);
  }
}
