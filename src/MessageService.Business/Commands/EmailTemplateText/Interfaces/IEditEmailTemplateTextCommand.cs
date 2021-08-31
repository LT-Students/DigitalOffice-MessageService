using System;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.EmailTemplateText;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.MessageService.Business.Commands.EmailTemplateText.Interfaces
{
  [AutoInject]
  public interface IEditEmailTemplateTextCommand
  {
    OperationResultResponse<bool> Execute(
      Guid emailTemplateTextId,
      JsonPatchDocument<EditEmailTemplateTextRequest> patch);
  }
}
