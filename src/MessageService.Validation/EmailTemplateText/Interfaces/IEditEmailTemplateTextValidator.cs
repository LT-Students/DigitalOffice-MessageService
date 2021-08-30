using FluentValidation;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.EmailTemplateText;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.MessageService.Validation.EmailTemplateText.Interfaces
{
  [AutoInject]
  public interface IEditEmailTemplateTextValidator : IValidator<JsonPatchDocument<EditEmailTemplateTextRequest>>
  {
  }
}
