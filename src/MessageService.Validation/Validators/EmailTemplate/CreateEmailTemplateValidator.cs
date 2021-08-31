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
        .NotEmpty();

      RuleFor(et => et.Type)
        .IsInEnum();

      RuleFor(et => et.AuthorId)
        .NotEmpty();

      RuleFor(et => et.EmailTemplateTexts)
        .NotNull();

      RuleForEach(et => et.EmailTemplateTexts)
        .Must(ett => ett != null)
        .ChildRules(ett =>
        {
          ett.RuleFor(ett => ett.Subject)
            .NotEmpty();

          ett.RuleFor(ett => ett.Text)
            .NotEmpty();

          ett.RuleFor(ett => ett.Language)
            .NotEmpty()
            .MaximumLength(2);
        });
    }
  }
}
