using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.MessageService.Business.EmailTemplatesCommands;
using LT.DigitalOffice.MessageService.Business.EmailTemplatesCommands.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto;
using LT.DigitalOffice.MessageService.Models.Dto.Enums;
using LT.DigitalOffice.MessageService.Models.Dto.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.MessageService.Business.UnitTests
{
    public class AddEmailTemplateCommandTests
    {
        private Mock<IEmailTemplateRepository> repositoryMock;
        private IAddEmailTemplateCommand command;
        private Mock<IMapper<EmailTemplateRequest, DbEmailTemplate>> mapperMock;
        private Mock<IAccessValidator> accessValidatorMock;

        private Guid emailId;
        private EmailTemplateRequest emailTemplate;
        private DbEmailTemplate dbEmailTemplate;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            emailId = Guid.NewGuid();
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

            dbEmailTemplate = new DbEmailTemplate
            {
                Id = emailId,
                Name = emailTemplate.Name,
                CreatedAt = DateTime.UtcNow,
                AuthorId = emailTemplate.AuthorId,
                IsActive = true
            };

            foreach (var templateText in emailTemplate.EmailTemplateTexts)
            {
                var dbEmailTemplateText = new DbEmailTemplateText();

                dbEmailTemplateText.Subject = templateText.Subject;
                dbEmailTemplateText.Text = templateText.Text;
                dbEmailTemplateText.Language = dbEmailTemplateText.Language;

                dbEmailTemplate.EmailTemplateTexts.Add(dbEmailTemplateText);
            }
        }

        [SetUp]
        public void SetUp()
        {
            repositoryMock = new Mock<IEmailTemplateRepository>();
            mapperMock = new Mock<IMapper<EmailTemplateRequest, DbEmailTemplate>>();
            accessValidatorMock = new Mock<IAccessValidator>();

            command = new AddEmailTemplateCommand(mapperMock.Object, repositoryMock.Object, accessValidatorMock.Object);
        }

        [Test]
        public void ShouldAddEmailTemplateCorrectly()
        {
            accessValidatorMock
                .Setup(x => x.HasRights(3))
                .Returns(true);

            repositoryMock
                .Setup(x => x.AddEmailTemplate(It.IsAny<DbEmailTemplate>()))
                .Returns(emailId);

            mapperMock
                .Setup(mapper => mapper.Map(It.IsAny<EmailTemplateRequest>()))
                .Returns(dbEmailTemplate);

            Assert.That(command.Execute(emailTemplate), Is.EqualTo(emailId));

            mapperMock.Verify();
            repositoryMock.Verify();
        }

        [Test]
        public void ShouldThrowExceptionWhenMapperThrowsIt()
        {
            accessValidatorMock
                .Setup(x => x.HasRights(3))
                .Returns(true);

            mapperMock
                .Setup(mapper => mapper.Map(It.IsAny<EmailTemplateRequest>()))
                .Throws<Exception>();

            Assert.Throws<Exception>(() => command.Execute(emailTemplate));

            mapperMock.Verify();
            repositoryMock.Verify(repository => repository.AddEmailTemplate(It.IsAny<DbEmailTemplate>()), Times.Never());
        }

        [Test]
        public void ShouldThrowExceptionWhenUserHasNoRight()
        {
            accessValidatorMock
                .Setup(x => x.HasRights(3))
                .Returns(false);

            Assert.Throws<Exception>(() => command.Execute(emailTemplate));
        }
    }
}