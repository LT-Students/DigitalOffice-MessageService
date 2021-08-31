using System;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.EmailTemplate;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.MessageService.Business.Commands.EmailTemplate.Interfaces
{
    [AutoInject]
    public interface IEditEmailTemplateCommand
    {
    OperationResultResponse<bool> Execute(
      Guid emailTemplateId,
      JsonPatchDocument<EditEmailTemplateRequest> patch);
  }
}
