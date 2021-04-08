using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.MessageService.Business.EmailTemplatesCommands;
using LT.DigitalOffice.MessageService.Business.EmailTemplatesCommands.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using Moq;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.MessageService.Business.UnitTests
{
    public class DisableEmailTemplateCommandTests
    {
        private Mock<IEmailTemplateRepository> repositoryMock;
        private IDisableEmailTemplateCommand command;
        private Mock<IAccessValidator> accessValidatorMock;

        private Guid emailTemplateId;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            repositoryMock = new Mock<IEmailTemplateRepository>();
            accessValidatorMock = new Mock<IAccessValidator>();

            command = new DisableEmailTemplateCommand(repositoryMock.Object, accessValidatorMock.Object);

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

            command.Execute(emailTemplateId);

            repositoryMock.Verify();
        }

        [Test]
        public void ShouldThrowExceptionWhenRepositoryThrowsIt()
        {
            repositoryMock.Setup(x => x.DisableEmailTemplate(It.IsAny<Guid>())).Throws(new Exception());

            Assert.Throws<Exception>(() => command.Execute(emailTemplateId));
            repositoryMock.Verify();
        }

        [Test]
        public void ShouldThrowExceptionWhenUserHasNoRight()
        {
            accessValidatorMock
                .Setup(x => x.HasRights(3))
                .Returns(false);

            Assert.Throws<Exception>(() => command.Execute(emailTemplateId));
        }
    }
}