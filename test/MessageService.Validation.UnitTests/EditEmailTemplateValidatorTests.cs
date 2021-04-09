using FluentValidation;
using FluentValidation.TestHelper;
using LT.DigitalOffice.MessageService.Models.Dto.Requests;
using LT.DigitalOffice.MessageService.Validation;
using NUnit.Framework;
using System;

namespace MessageService.Validation.UnitTests
{
    public class EditEmailTemplateValidatorTests
    {
        private IValidator<EditEmailTemplateRequest> validator;
        private EditEmailTemplateRequest emailTemplate;

        [SetUp]
        public void SetUp()
        {
            validator = new EditEmailTemplateValidator();

            emailTemplate = new EditEmailTemplateRequest
            {
                Id = Guid.NewGuid(),
            };
        }

        [Test]
        public void ShouldThrowValidationExceptionWhenEmailTemplateIdIsNull()
        {
            validator.ShouldHaveValidationErrorFor(x => x.Id, Guid.Empty);
        }

        [Test]
        public void ShouldNotHaveAnyValidationErrorsWhenRequestIsValid()
        {
            validator.TestValidate(emailTemplate).ShouldNotHaveAnyValidationErrors();
        }
    }
}
