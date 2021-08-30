using FluentValidation;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.EmailTemplate;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.MessageService.Validation.EmailTemplate.Interfaces
{
  [AutoInject]
  public interface IEditEmailTemplateValidator : IValidator<JsonPatchDocument<EditEmailTemplateRequest>>
  {
  }
}
