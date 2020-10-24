using LT.DigitalOffice.Kernel.AccessValidator.Interfaces;
using LT.DigitalOffice.MessageService.Business.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto;
using Moq;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.MessageService.Business.UnitTests
{
    public class AddEmailTemplateCommandTests
    {
        private Mock<IEmailTemplateRepository> repositoryMock;
        private IAddEmailTemplateCommand command;
        private Mock<IMapper<EmailTemplate, DbEmailTemplate>> mapperMock;
        private Mock<IAccessValidator> accessValidatorMock;

        private Guid emailId;
        private EmailTemplate emailTemplate;
        private DbEmailTemplate dbEmailTemplate;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            emailId = Guid.NewGuid();
            emailTemplate = new EmailTemplate
            {
                Subject = "Subject",
                Body = "Body",
                AuthorId = Guid.NewGuid()
            };

            dbEmailTemplate = new DbEmailTemplate
            {
                Id = emailId,
                Subject = emailTemplate.Subject,
                Body = emailTemplate.Body,
                AuthorId = emailTemplate.AuthorId,
                IsActive = true
            };
        }

        [SetUp]
        public void SetUp()
        {
            repositoryMock = new Mock<IEmailTemplateRepository>();
            mapperMock = new Mock<IMapper<EmailTemplate, DbEmailTemplate>>();
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
                .Setup(mapper => mapper.Map(It.IsAny<EmailTemplate>()))
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
                .Setup(mapper => mapper.Map(It.IsAny<EmailTemplate>()))
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