using FluentValidation;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.EmailTemplate;

namespace LT.DigitalOffice.MessageService.Validation.Validators.EmailTemplate.Interfaces
{
  [AutoInject]
  public interface IEditEmailTemplateValidator : IValidator<EditEmailTemplateRequest>
  {
  }
}
