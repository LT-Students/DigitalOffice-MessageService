using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Channel.Filters;
using LT.DigitalOffice.MessageService.Models.Dto.Responses.Channel;

namespace LT.DigitalOffice.MessageService.Business.Commands.Channels.Interfaces
{
  [AutoInject]
  public interface IGetChannelCommand
  {
    Task<OperationResultResponse<ChannelInfo>> ExecuteAsync(GetChannelFilter filter);
  }
}
