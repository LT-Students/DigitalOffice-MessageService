using FluentValidation;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.EmailTemplate;
using LT.DigitalOffice.MessageService.Validation.Validators.EmailTemplateText.Interfaces;

namespace LT.DigitalOffice.MessageService.Validation.Validators.EmailTemplateText
{
  public class CreateEmailTemplateTextValidator : AbstractValidator<EmailTemplateTextRequest>, ICreateEmailTemplateTextValidator
  {
    public CreateEmailTemplateTextValidator()
    {
      RuleFor(x => x.EmailTemplateId)
        .NotEmpty().WithMessage("Email template id must not be empty.");

      RuleFor(x => x.Subject)
        .NotEmpty().WithMessage("Subject must not be empty.");

      RuleFor(x => x.Text)
        .NotEmpty().WithMessage("Text must not be empty.");

      RuleFor(x => x.Language)
        .NotEmpty().WithMessage("Language must not be empty.")
        .MaximumLength(2).WithMessage("Language is to long.");
    }
  }
}
