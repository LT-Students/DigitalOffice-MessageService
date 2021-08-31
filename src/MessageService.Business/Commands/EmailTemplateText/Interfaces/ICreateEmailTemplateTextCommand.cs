using System;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.EmailTemplate;

namespace LT.DigitalOffice.MessageService.Business.Commands.EmailTemplateText.Interfaces
{
  [AutoInject]
  public interface ICreateEmailTemplateTextCommand
  {
    OperationResultResponse<Guid?> Execute(EmailTemplateTextRequest request);
  }
}
