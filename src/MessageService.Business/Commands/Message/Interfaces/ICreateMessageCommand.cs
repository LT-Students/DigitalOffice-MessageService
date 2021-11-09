using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Models.Dto.Enums;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Message;

namespace LT.DigitalOffice.MessageService.Business.Commands.Message.Interfaces
{
  [AutoInject]
  public interface ICreateMessageCommand
  {
    Task<OperationResultResponse<StatusType>> ExecuteAsync(CreateMessageRequest request);
  }
}
