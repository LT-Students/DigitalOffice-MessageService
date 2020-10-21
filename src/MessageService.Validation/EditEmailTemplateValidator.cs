using FluentValidation;
using LT.DigitalOffice.MessageService.Models.Dto;
using System;

namespace LT.DigitalOffice.MessageService.Validation
{
    public class EditEmailTemplateValidator : AbstractValidator<EditEmailTemplateRequest>
    {
        public EditEmailTemplateValidator()
        {
            RuleFor(et => et.Id)
                .NotEmpty();

            RuleFor(et => et.Subject)
                .NotEmpty()
                .MaximumLength(120)
                .WithMessage("Subject too long");
        }
    }
}
