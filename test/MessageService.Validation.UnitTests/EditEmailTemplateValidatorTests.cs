using FluentValidation;
using FluentValidation.TestHelper;
using LT.DigitalOffice.MessageService.Models.Dto.Enums;
using LT.DigitalOffice.MessageService.Models.Dto.Models;
using LT.DigitalOffice.MessageService.Models.Dto.Requests;
using LT.DigitalOffice.MessageService.Validation;
using NUnit.Framework;
using System;
using System.Collections.Generic;

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
                Name = "Pattern name",
                Type = EmailTemplateType.Greeting,
                EmailTemplateTexts = new List<EmailTemplateTextInfo>
                {
                    new EmailTemplateTextInfo
                    {
                        Subject = "Subject",
                        Text = "Email text",
                        Language = "en"
                    }
                }
            };
        }

        [Test]
        public void ShouldThrowValidationExceptionWhenEmailTemplateIdIsNull()
        {
            validator.ShouldHaveValidationErrorFor(x => x.Id, Guid.Empty);
        }

        [Test]
        public void ShouldThrowValidationExceptionWhenTemplateNameIsEmpty()
        {
            validator.ShouldHaveValidationErrorFor(x => x.Name, "");
        }

        [Test]
        public void ShouldThrowValidationExceptionWhenTemplateTypeIsEmpty()
        {
            validator.ShouldHaveValidationErrorFor(x => x.Type, (EmailTemplateType)7);
        }

        [Test]
        public void ShouldNotHaveAnyValidationErrorsWhenRequestIsValid()
        {
            validator.TestValidate(emailTemplate).ShouldNotHaveAnyValidationErrors();
        }
    }
}
