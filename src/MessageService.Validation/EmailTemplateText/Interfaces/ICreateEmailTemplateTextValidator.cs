using FluentValidation;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.EmailTemplate;

namespace LT.DigitalOffice.MessageService.Validation.EmailTemplateText.Interfaces
{
  [AutoInject]
  public interface ICreateEmailTemplateTextValidator : IValidator<EmailTemplateTextRequest>
  {
  }
}
