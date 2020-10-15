using FluentValidation;
using FluentValidation.TestHelper;
using LT.DigitalOffice.MessageService.Models.Dto;
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
                Subject = "Subject",
                Body = "Body",
            };
        }

        [Test]
        public void ShouldThrowValidationExceptionWhenEmailTemplateIdIsNull()
        {
            validator.ShouldHaveValidationErrorFor(x => x.Id, Guid.Empty);
        }

        [Test]
        public void ShouldThrowValidationExceptionWhenSubjectIsEmpty()
        {
            validator.ShouldHaveValidationErrorFor(x => x.Subject, "");
        }

        [Test]
        public void ShouldThrowValidationExceptionWhenSubjectIsTooLong()
        {
            var subject = emailTemplate.Subject.PadLeft(300);

            validator.ShouldHaveValidationErrorFor(x => x.Subject, subject);
        }

        [Test]
        public void ShouldThrowValidationExceptionWhenBodyIsEmpty()
        {
            var body = String.Empty;

            validator.ShouldHaveValidationErrorFor(x => x.Body, body);
        }

        [Test]
        public void ShouldNotHaveAnyValidationErrorsWhenRequestIsValid()
        {
            validator.TestValidate(emailTemplate).ShouldNotHaveAnyValidationErrors();
        }
    }
}
