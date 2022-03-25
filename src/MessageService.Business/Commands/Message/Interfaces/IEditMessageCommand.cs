using System;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Message;

namespace LT.DigitalOffice.MessageService.Business.Commands.Message.Interfaces
{
  public interface IEditMessageCommand
  {
    Task<OperationResultResponse<bool>> ExecuteAsync(Guid messageId, EditMessageRequest request);
  }
}
