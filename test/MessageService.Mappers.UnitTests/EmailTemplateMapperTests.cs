using LT.DigitalOffice.MessageService.Mappers.EmailMappers;
using LT.DigitalOffice.MessageService.Mappers.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Enums;
using LT.DigitalOffice.MessageService.Models.Dto.Models;
using LT.DigitalOffice.MessageService.Models.Dto.Requests;
using LT.DigitalOffice.UnitTestKernel;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.MessageService.Mappers.UnitTests
{
    public class EmailTemplateMapperTests
    {
        private IMapper<EmailTemplateRequest, DbEmailTemplate> mapper;
        private IMapper<EditEmailTemplateRequest, DbEmailTemplate> editMapper;

        private EmailTemplateRequest emailTemplate;
        private DbEmailTemplate expectedDbEmailTemplate;
        private EmailTemplateTextInfo emailTemplateTextInfo;
        private EditEmailTemplateRequest editEmailTemplateRequest;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            mapper = new EmailTemplateMapper();
            editMapper = new EmailTemplateMapper();

            emailTemplate = new EmailTemplateRequest
            {
                Name = "Pattern name",
                Type = EmailTemplateType.Greeting,
                AuthorId = Guid.NewGuid(),
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

            emailTemplateTextInfo = new EmailTemplateTextInfo()
            {
                Subject = "New subject",
                Text = "New email text",
                Language = "ru"
            };

            editEmailTemplateRequest = new EditEmailTemplateRequest()
            {
                Name = "New pattern name",
                Type = EmailTemplateType.Greeting,
                EmailTemplateTexts = new List<EmailTemplateTextInfo>
                {
                    new EmailTemplateTextInfo
                    {
                        Subject = emailTemplateTextInfo.Subject,
                        Text = emailTemplateTextInfo.Text,
                        Language = emailTemplateTextInfo.Language
                    }
                }
            };

            expectedDbEmailTemplate = new DbEmailTemplate
            {
                Name = emailTemplate.Name,
                CreatedAt = DateTime.UtcNow,
                AuthorId = emailTemplate.AuthorId,
                Type = (int)emailTemplate.Type,
                IsActive = true
            };

            foreach (var templateText in emailTemplate.EmailTemplateTexts)
            {
                var dbEmailTemplateText = new DbEmailTemplateText()
                {
                    Subject = templateText.Subject,
                    Text = templateText.Text,
                    Language = templateText.Language
                };

                expectedDbEmailTemplate.EmailTemplateTexts.Add(dbEmailTemplateText);
            }
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWhenEmailTemplateIsNull()
        {
            emailTemplate = null;

            Assert.Throws<ArgumentNullException>(() => mapper.Map(emailTemplate));
        }

        [Test]
        public void ShouldReturnRightModelWhenEmailTemplateIsMapped()
        {
            var resultDbEmailTemplate = mapper.Map(emailTemplate);
            expectedDbEmailTemplate.Id = resultDbEmailTemplate.Id;
            expectedDbEmailTemplate.CreatedAt = resultDbEmailTemplate.CreatedAt;
            expectedDbEmailTemplate.EmailTemplateTexts.ElementAt(0).Id =
                resultDbEmailTemplate.EmailTemplateTexts.ElementAt(0).Id;
            expectedDbEmailTemplate.EmailTemplateTexts.ElementAt(0).EmailTemplateId =
                resultDbEmailTemplate.Id;

            SerializerAssert.AreEqual(expectedDbEmailTemplate, resultDbEmailTemplate);
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWhenEditEmailTemplateRequestIsNull()
        {
            EditEmailTemplateRequest newEditEmailTemplateRequest = null;

            Assert.Throws<ArgumentNullException>(() => editMapper.Map(newEditEmailTemplateRequest));
        }

        [Test]
        public void ShouldReturnRightModelSuccessful()
        {
            var dbEmailTemplate = editMapper.Map(editEmailTemplateRequest);

            var expectedDbEmailTemplate = new DbEmailTemplate
            {
                Id = editEmailTemplateRequest.Id,
                Name = editEmailTemplateRequest.Name,
                Type = (int)editEmailTemplateRequest.Type
            };

            SerializerAssert.AreEqual(expectedDbEmailTemplate, dbEmailTemplate);
        }
    }
}