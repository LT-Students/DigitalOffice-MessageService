using FluentValidation;
using LT.DigitalOffice.MessageService.Models.Dto.Requests;

namespace LT.DigitalOffice.MessageService.Validation
{
    public class EditEmailTemplateValidator : AbstractValidator<EditEmailTemplateRequest>
    {
        public EditEmailTemplateValidator()
        {
            RuleFor(et => et.Id)
                .NotEmpty();
        }
    }
}
