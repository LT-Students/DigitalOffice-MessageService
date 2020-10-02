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
    public class RemoveEmailTemplateCommandTests
    {
        private Mock<IEmailTemplateRepository> repositoryMock;
        private IDisableEmailTemplateCommand command;
        private Mock<IAccessValidator> accessValidatorMock;

        private Guid requestingUserId;
        private Guid emailTemplateId;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            repositoryMock = new Mock<IEmailTemplateRepository>();
            accessValidatorMock = new Mock<IAccessValidator>();

            command = new DisableEmailTemplateCommand(repositoryMock.Object, accessValidatorMock.Object);

            requestingUserId = Guid.NewGuid();
            emailTemplateId = Guid.NewGuid();
        }

        [Test]
        public void ShouldRemoveEmailTemplateCorrectly()
        {
            accessValidatorMock
                .Setup(x => x.HasRights(3))
                .Returns(true);

            repositoryMock
                .Setup(x => x.DisableEmailTemplate(It.IsAny<Guid>()));

            command.Execute(emailTemplateId, requestingUserId);

            repositoryMock.Verify();
        }

        [Test]
        public void ShouldThrowExceptionWhenRepositoryThrowsIt()
        {
            repositoryMock.Setup(x => x.DisableEmailTemplate(It.IsAny<Guid>())).Throws(new Exception());

            Assert.Throws<Exception>(() => command.Execute(emailTemplateId, requestingUserId));
            repositoryMock.Verify();
        }

        [Test]
        public void ShouldThrowExceptionWhenUserHasNoRight()
        {
            accessValidatorMock
                .Setup(x => x.HasRights(3))
                .Returns(false);

            Assert.Throws<Exception>(() => command.Execute(emailTemplateId, requestingUserId));
        }
    }
}