using LT.DigitalOffice.MessageService.Mappers.Db;
using LT.DigitalOffice.MessageService.Mappers.Db.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.EmailTemplate;
using LT.DigitalOffice.Models.Broker.Enums;
using LT.DigitalOffice.UnitTestKernel;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.MessageService.Mappers.UnitTests.EmailTemplate
{
    class EditDbEmailTemplateMapperTests
    {
        private IEditDbEmailTemplateMapper _mapper;

        private EmailTemplateRequest _emailTemplate;
        private DbEmailTemplate _expectedDbEmailTemplate;
        private EmailTemplateTextRequest _emailTemplateTextInfo;
        private EditEmailTemplateRequest _editEmailTemplateRequest;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _mapper = new EditDbEmailTemplateMapper();

            _emailTemplate = new EmailTemplateRequest
            {
                Name = "Pattern name",
                Type = EmailTemplateType.Greeting,
                AuthorId = Guid.NewGuid(),
                EmailTemplateTexts = new List<EmailTemplateTextRequest>
                {
                    new EmailTemplateTextRequest
                    {
                        Subject = "Subject",
                        Text = "Email text",
                        Language = "en"
                    }
                }
            };

            _emailTemplateTextInfo = new EmailTemplateTextRequest()
            {
                Subject = "New subject",
                Text = "New email text",
                Language = "ru"
            };

            _editEmailTemplateRequest = new EditEmailTemplateRequest()
            {
                Name = "New pattern name",
                Type = EmailTemplateType.Greeting,
                EmailTemplateTexts = new List<EmailTemplateTextRequest>
                {
                    new EmailTemplateTextRequest
                    {
                        Subject = _emailTemplateTextInfo.Subject,
                        Text = _emailTemplateTextInfo.Text,
                        Language = _emailTemplateTextInfo.Language
                    }
                }
            };

            _expectedDbEmailTemplate = new DbEmailTemplate
            {
                Name = _emailTemplate.Name,
                CreatedAtUtc = DateTime.UtcNow,
                CreatedBy = _emailTemplate.AuthorId,
                Type = (int)_emailTemplate.Type,
                IsActive = true
            };
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWhenEditEmailTemplateRequestIsNull()
        {
            EditEmailTemplateRequest newEditEmailTemplateRequest = null;

            Assert.Throws<ArgumentNullException>(() => _mapper.Map(newEditEmailTemplateRequest));
        }

        [Test]
        public void ShouldReturnRightModelSuccessful()
        {
            var dbEmailTemplate = _mapper.Map(_editEmailTemplateRequest);

            var expectedDbEmailTemplate = new DbEmailTemplate
            {
                Id = _editEmailTemplateRequest.Id,
                Name = _editEmailTemplateRequest.Name,
                Type = (int)_editEmailTemplateRequest.Type
            };

            SerializerAssert.AreEqual(expectedDbEmailTemplate, dbEmailTemplate);
        }
    }
}
