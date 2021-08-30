using LT.DigitalOffice.MessageService.Mappers.Models;
using LT.DigitalOffice.MessageService.Mappers.Models.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Models.Emails;
using LT.DigitalOffice.UnitTestKernel;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.MessageService.Mappers.UnitTests.EmailTemplate
{
    public class EmailTemplateTextInfoMapperTests
    {
        private IEmailTemplateTextInfoMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            _mapper = new EmailTemplateTextInfoMapper();
        }

        [Test]
        public void ShouldMapSuccessfuly()
        {
            DbEmailTemplateText dbEmailTemplate = new DbEmailTemplateText
            {
                Id = Guid.NewGuid(),
                EmailTemplateId = Guid.NewGuid(),
                Language = "Language",
                Subject = "Subject",
                Text = "text"
            };

            EmailTemplateTextInfo emailTemplate = new EmailTemplateTextInfo
            {
                Id = dbEmailTemplate.Id,
                Language = dbEmailTemplate.Language,
                Subject = dbEmailTemplate.Subject,
                Text = dbEmailTemplate.Text
            };

            SerializerAssert.AreEqual(emailTemplate, _mapper.Map(dbEmailTemplate));
        }
    }
}
