using LT.DigitalOffice.MessageService.Mappers.Db;
using LT.DigitalOffice.MessageService.Mappers.Db.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Enums;
using LT.DigitalOffice.MessageService.Models.Dto.Models;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.EmailTemplate;
using LT.DigitalOffice.UnitTestKernel;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.MessageService.Mappers.UnitTests
{
    public class DbEmailTemplateMapperTests
    {
        private IDbEmailTemplateMapper _mapper;

        private EmailTemplateRequest _emailTemplate;
        private DbEmailTemplate _expectedDbEmailTemplate;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _mapper = new DbEmailTemplateMapper();

            _emailTemplate = new EmailTemplateRequest
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

            _expectedDbEmailTemplate = new DbEmailTemplate
            {
                Name = _emailTemplate.Name,
                CreatedAt = DateTime.UtcNow,
                AuthorId = _emailTemplate.AuthorId,
                Type = (int)_emailTemplate.Type,
                IsActive = true
            };

            foreach (var templateText in _emailTemplate.EmailTemplateTexts)
            {
                var dbEmailTemplateText = new DbEmailTemplateText()
                {
                    Subject = templateText.Subject,
                    Text = templateText.Text,
                    Language = templateText.Language
                };

                _expectedDbEmailTemplate.EmailTemplateTexts.Add(dbEmailTemplateText);
            }
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWhenEmailTemplateIsNull()
        {
            _emailTemplate = null;

            Assert.Throws<ArgumentNullException>(() => _mapper.Map(_emailTemplate));
        }

        [Test]
        public void ShouldReturnRightModelWhenEmailTemplateIsMapped()
        {
            var resultDbEmailTemplate = _mapper.Map(_emailTemplate);
            _expectedDbEmailTemplate.Id = resultDbEmailTemplate.Id;
            _expectedDbEmailTemplate.CreatedAt = resultDbEmailTemplate.CreatedAt;
            _expectedDbEmailTemplate.EmailTemplateTexts.ElementAt(0).Id =
                resultDbEmailTemplate.EmailTemplateTexts.ElementAt(0).Id;
            _expectedDbEmailTemplate.EmailTemplateTexts.ElementAt(0).EmailTemplateId =
                resultDbEmailTemplate.Id;

            SerializerAssert.AreEqual(_expectedDbEmailTemplate, resultDbEmailTemplate);
        }
    }
}
