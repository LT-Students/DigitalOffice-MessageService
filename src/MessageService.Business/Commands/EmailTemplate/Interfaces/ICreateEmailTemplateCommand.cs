using System;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.EmailTemplate;

namespace LT.DigitalOffice.MessageService.Business.Commands.EmailTemplate.Interfaces
{
  [AutoInject]
  public interface ICreateEmailTemplateCommand
  {
    OperationResultResponse<Guid?> Execute(EmailTemplateRequest request);
  }
}
