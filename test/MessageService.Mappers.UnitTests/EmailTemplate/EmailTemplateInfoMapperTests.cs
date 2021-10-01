using LT.DigitalOffice.MessageService.Mappers.Models;
using LT.DigitalOffice.MessageService.Mappers.Models.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Models.Emails;
using LT.DigitalOffice.Models.Broker.Enums;
using LT.DigitalOffice.UnitTestKernel;
using Moq.AutoMock;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.DigitalOffice.MessageService.Mappers.UnitTests.EmailTemplate
{
    public class EmailTemplateInfoMapperTests
    {
        private AutoMocker _mocker;
        private IEmailTemplateInfoMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            _mocker = new();
            _mapper = _mocker.CreateInstance<EmailTemplateInfoMapper>();
        }

        [Test]
        public void ShouldMapSuccessfuly()
        {
            DbEmailTemplateText dbTemplateText1 = new DbEmailTemplateText
            {
                Id = Guid.NewGuid(),
                EmailTemplateId = Guid.NewGuid(),
                Language = "Language",
                Subject = "Subject",
                Text = "text"
            };

            DbEmailTemplateText dbTemplateText2 = new DbEmailTemplateText
            {
                Id = Guid.NewGuid(),
                EmailTemplateId = Guid.NewGuid(),
                Language = "Language",
                Subject = "Subject",
                Text = "text"
            };

            DbEmailTemplate dbEmailTemplate = new DbEmailTemplate
            {
                Id = Guid.NewGuid(),
                CreatedBy = Guid.NewGuid(),
                Name = "Name",
                CreatedAtUtc = DateTime.UtcNow,
                IsActive = true,
                Type = (int)EmailTemplateType.Greeting,
                EmailTemplateTexts = new List<DbEmailTemplateText>()
                {
                    dbTemplateText1,
                    dbTemplateText2
                }
            };

            EmailTemplateTextInfo templateText1 = new EmailTemplateTextInfo
            {
                Id = dbTemplateText1.Id,
                Language = dbTemplateText1.Language,
                Subject = dbTemplateText1.Subject,
                Text = dbTemplateText1.Text
            };

            EmailTemplateTextInfo templateText2 = new EmailTemplateTextInfo
            {
                Id = dbTemplateText2.Id,
                Language = dbTemplateText2.Language,
                Subject = dbTemplateText2.Subject,
                Text = dbTemplateText2.Text
            };

            EmailTemplateInfo emailTemplateInfo = new EmailTemplateInfo
            {
                Id = dbEmailTemplate.Id,
                CreatedBy = dbEmailTemplate.CreatedBy,
                Name = dbEmailTemplate.Name,
                CreatedAtUtc = dbEmailTemplate.CreatedAtUtc,
                IsActive = dbEmailTemplate.IsActive,
                Type = EmailTemplateType.Greeting.ToString(),
                Texts = new List<EmailTemplateTextInfo>()
                {
                    templateText1,
                    templateText2
                }
            };

            _mocker
                .Setup<IEmailTemplateTextInfoMapper, EmailTemplateTextInfo>(x => x.Map(dbTemplateText1))
                .Returns(templateText1);

            _mocker
                .Setup<IEmailTemplateTextInfoMapper, EmailTemplateTextInfo>(x => x.Map(dbTemplateText2))
                .Returns(templateText2);

            SerializerAssert.AreEqual(emailTemplateInfo, _mapper.Map(dbEmailTemplate));
        }
    }
}
