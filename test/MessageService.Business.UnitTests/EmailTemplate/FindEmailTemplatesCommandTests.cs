using LT.DigitalOffice.MessageService.Business.Commands.EmailTemplate;
using LT.DigitalOffice.MessageService.Business.Commands.EmailTemplate.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Models.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Models.Emails;
using LT.DigitalOffice.MessageService.Models.Dto.Responses;
using LT.DigitalOffice.Models.Broker.Enums;
using LT.DigitalOffice.UnitTestKernel;
using Moq.AutoMock;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.MessageService.Business.UnitTests.EmailTemplate
{
    public class FindEmailTemplatesCommandTests
    {
        private AutoMocker _mocker;
        private IFindEmailTemplateCommand _command;

        /*[SetUp]
        public void SetUp()
        {
            _mocker = new();
            _command = _mocker.CreateInstance<FindEmailTemplateCommand>();
        }

        [Test]
        public void ShouldThrowExceptionWhenRepositoryThrow()
        {
            int total = 2;
            int skip = 0;
            int take = 2;
            bool includeDeactivated = true;

            _mocker
                .Setup<IEmailTemplateRepository, List<DbEmailTemplate>>(
                    x => x.Find(skip, take, includeDeactivated, out total))
                .Throws(new Exception());

            Assert.Throws<Exception>(() => _command.Execute(skip, take, includeDeactivated));
        }

        [Test]
        public void ShouldReturnTemplatesSuccessfuly()
        {
            int total = 3;
            int skip = 0;
            int take = 2;
            bool includeDeactivated = true;

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
                .Setup<IEmailTemplateInfoMapper, EmailTemplateInfo>(x => x.Map(dbEmailTemplate))
                .Returns(emailTemplateInfo);

            _mocker
                .Setup<IEmailTemplateRepository, List<DbEmailTemplate>>(
                    x => x.Find(skip, take, includeDeactivated, out total))
                .Returns(new List<DbEmailTemplate> { dbEmailTemplate });

            EmailTemplatesResponse response = new EmailTemplatesResponse
            {
                TotalCount = total,
                Emails = new List<EmailTemplateInfo> { emailTemplateInfo }
            };

            SerializerAssert.AreEqual(response, _command.Execute(skip, take, includeDeactivated));
        }*/

    }
}
