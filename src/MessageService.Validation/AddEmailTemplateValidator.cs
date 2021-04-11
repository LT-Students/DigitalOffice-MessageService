using FluentValidation;
using LT.DigitalOffice.MessageService.Models.Dto.Requests;

namespace LT.DigitalOffice.MessageService.Validation
{
    public class AddEmailTemplateValidator : AbstractValidator<EmailTemplateRequest>
    {
        public AddEmailTemplateValidator()
        {
            RuleFor(et => et.Name)
                .NotEmpty();

            RuleFor(et => et.Type)
                    .NotEmpty();

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
                        .NotEmpty();
                });
        }
    }
}
