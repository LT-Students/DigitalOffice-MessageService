using FluentValidation;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.EmailTemplate;
using LT.DigitalOffice.MessageService.Validation.Validators.EmailTemplate.Interfaces;

namespace LT.DigitalOffice.MessageService.Validation.Validators.EmailTemplate
{
  public class CreateEmailTemplateValidator : AbstractValidator<EmailTemplateRequest>, ICreateEmailTemplateValidator
  {
    public CreateEmailTemplateValidator()
    {
      RuleFor(et => et.Name)
        .NotEmpty().WithMessage("Email template name must not be empty.");

      RuleFor(et => et.Type)
        .IsInEnum().WithMessage("Incorrect Email template type.");

      RuleFor(et => et.EmailTemplateTexts)
        .NotNull();

      RuleForEach(et => et.EmailTemplateTexts)
        .Must(ett => ett != null)
        .ChildRules(ett =>
        {
          ett.RuleFor(ett => ett.Subject)
            .NotEmpty().WithMessage("Subject must not be empty");

          ett.RuleFor(ett => ett.Text)
            .NotEmpty().WithMessage("Text must not be empty");

          ett.RuleFor(ett => ett.Language)
            .NotEmpty().WithMessage("Language must not be empty")
            .MaximumLength(2).WithMessage("Language is to long");
        });
    }
  }
}
