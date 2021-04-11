﻿using FluentValidation;
using LT.DigitalOffice.MessageService.Models.Dto;
using LT.DigitalOffice.MessageService.Models.Dto.Requests;

namespace LT.DigitalOffice.MessageService.Validation
{
    public class EditEmailTemplateValidator : AbstractValidator<EditEmailTemplateRequest>
    {
        public EditEmailTemplateValidator()
        {
            RuleFor(et => et.Id)
                .NotEmpty();

            RuleFor(et => et.Name)
                .NotEmpty();

            RuleFor(et => et.Type)
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
