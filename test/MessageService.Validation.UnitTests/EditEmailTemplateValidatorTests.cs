using FluentValidation;
using FluentValidation.TestHelper;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.EmailTemplate;
using LT.DigitalOffice.MessageService.Validation.Validators.EmailTemplate;
using LT.DigitalOffice.Models.Broker.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.MessageService.Validation.UnitTests
{
  public class EditEmailTemplateValidatorTests
    {
        private IValidator<EditEmailTemplateRequest> _validator;
        private EditEmailTemplateRequest _emailTemplateRequest;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _validator = new EditEmailTemplateValidator();

            _emailTemplateRequest = new EditEmailTemplateRequest
            {
                Id = Guid.NewGuid(),
                Name = "Pattern name",
                Type = EmailTemplateType.Greeting,
            };
        }

        [Test]
        public void ShouldThrowExceptionWhenEmailTemplateIdIsEmpty()
        {
            _validator.ShouldHaveValidationErrorFor(x => x.Id, Guid.Empty);
        }

        [Test]
        public void ShouldThrowExceptionWhenEmailTemplateNameIsEmpty()
        {
            _validator.ShouldHaveValidationErrorFor(x => x.Name, "");
        }

        [Test]
        public void ShouldThrowExceptionWhenEmailTemplateTypeIsEmpty()
        {
            _validator.ShouldHaveValidationErrorFor(x => x.Type, (EmailTemplateType)4);
        }

        [Test]
        public void ShouldThrowExceptionWhenTemplateTextsIsNull()
        {
            IEnumerable<EmailTemplateTextRequest> templateTexts = null;

            _validator.ShouldHaveValidationErrorFor(x => x.EmailTemplateTexts, templateTexts);
        }

        [Test]
        public void ShouldThrowExceptionWhenTemplateTextIsNull()
        {
            List<EmailTemplateTextRequest> templateTexts = null;

            _validator.ShouldHaveValidationErrorFor(x => x.EmailTemplateTexts, templateTexts);
        }

        [Test]
        public void ShouldThrowExceptionWhenSubjectIsEmpty()
        {
            _emailTemplateRequest.EmailTemplateTexts = new List<EmailTemplateTextRequest>
            {
                new EmailTemplateTextRequest
                {
                    Subject = "",
                    Language = "ru",
                    Text = "text"
                }
            };

            _validator.TestValidate(_emailTemplateRequest).ShouldHaveAnyValidationError();
        }

        [Test]
        public void ShouldThrowExceptionWhenLanguageIsEmpty()
        {
            _emailTemplateRequest.EmailTemplateTexts = new List<EmailTemplateTextRequest>
            {
                new EmailTemplateTextRequest
                {
                    Subject = "subject",
                    Language = "",
                    Text = "text"
                }
            };

            _validator.TestValidate(_emailTemplateRequest).ShouldHaveAnyValidationError();
        }

        [Test]
        public void ShouldThrowExceptionWhenLanguageMoreThenTwoChars()
        {
            _emailTemplateRequest.EmailTemplateTexts = new List<EmailTemplateTextRequest>
            {
                new EmailTemplateTextRequest
                {
                    Subject = "subject",
                    Language = "english",
                    Text = "text"
                }
            };

            _validator.TestValidate(_emailTemplateRequest).ShouldHaveAnyValidationError();
        }

        [Test]
        public void ShouldThrowExceptionWhenTextIsEmpty()
        {
            _emailTemplateRequest.EmailTemplateTexts = new List<EmailTemplateTextRequest>
            {
                new EmailTemplateTextRequest
                {
                    Subject = "subject",
                    Language = "en",
                    Text = ""
                }
            };

            _validator.TestValidate(_emailTemplateRequest).ShouldHaveAnyValidationError();
        }

        [Test]
        public void ShouldNotHaveAnyValidationErrorsWhenRequestIsValid()
        {
            _emailTemplateRequest.EmailTemplateTexts = new List<EmailTemplateTextRequest>
            {
                new EmailTemplateTextRequest
                {
                    Subject = "Subject",
                    Text = "Email text",
                    Language = "en"
                }
            };

            _validator.TestValidate(_emailTemplateRequest).ShouldNotHaveAnyValidationErrors();
        }
    }
}
