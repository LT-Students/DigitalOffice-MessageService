using FluentValidation;
using FluentValidation.TestHelper;
using LT.DigitalOffice.MessageService.Models.Dto.Enums;
using LT.DigitalOffice.MessageService.Models.Dto.Models.Emails;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.EmailTemplate;
using LT.DigitalOffice.Models.Broker.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.MessageService.Validation.UnitTests
{
    class CreateEmailTemplateValidatorTests
    {
        private IValidator<EmailTemplateRequest> _validator;
        private EmailTemplateRequest _emailTemplateRequest;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _validator = new CreateEmailTemplateValidator();

            _emailTemplateRequest = new EmailTemplateRequest
            {
                Name = "Pattern name",
                AuthorId = Guid.NewGuid(),
                Type = EmailTemplateType.Greeting,
            };
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
        public void ShouldThrowExceptionWhenEmailTemplateAuthorIsEmpty()
        {
            _validator.ShouldHaveValidationErrorFor(x => x.AuthorId, Guid.Empty);
        }

        [Test]
        public void ShouldThrowExceptionWhenTemplateTextsIsNull()
        {
            IEnumerable<EmailTemplateTextInfo> templateTexts= null;

            _validator.ShouldHaveValidationErrorFor(x => x.EmailTemplateTexts, templateTexts);
        }

        [Test]
        public void ShouldThrowExceptionWhenTemplateTextIsNull()
        {
            List<EmailTemplateTextInfo> templateTexts = null;

            _validator.ShouldHaveValidationErrorFor(x => x.EmailTemplateTexts, templateTexts);
        }

        [Test]
        public void ShouldThrowExceptionWhenSubjectIsEmpty()
        {
            _emailTemplateRequest.EmailTemplateTexts = new List<EmailTemplateTextInfo>
            {
                new EmailTemplateTextInfo
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
            _emailTemplateRequest.EmailTemplateTexts = new List<EmailTemplateTextInfo>
            {
                new EmailTemplateTextInfo
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
            _emailTemplateRequest.EmailTemplateTexts = new List<EmailTemplateTextInfo>
            {
                new EmailTemplateTextInfo
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
            _emailTemplateRequest.EmailTemplateTexts = new List<EmailTemplateTextInfo>
            {
                new EmailTemplateTextInfo
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
            _emailTemplateRequest.EmailTemplateTexts = new List<EmailTemplateTextInfo>
            {
                new EmailTemplateTextInfo
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
