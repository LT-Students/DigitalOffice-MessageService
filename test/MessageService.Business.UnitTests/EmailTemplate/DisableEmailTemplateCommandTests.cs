using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Business.Commands.EmailTemplate;
using LT.DigitalOffice.MessageService.Business.Commands.EmailTemplate.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.UnitTestKernel;
using Moq;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.MessageService.Business.UnitTests.EmailTemplate
{
    public class DisableEmailTemplateCommandTests
    {
        private IDisableEmailTemplateCommand _command;
        private Mock<IAccessValidator> _accessValidatorMock;
        private Mock<IEmailTemplateRepository> _repositoryMock;

        private Guid _emailTemplateId;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _repositoryMock = new Mock<IEmailTemplateRepository>();
            _accessValidatorMock = new Mock<IAccessValidator>();

            _command = new DisableEmailTemplateCommand(_accessValidatorMock.Object, _repositoryMock.Object);

            _emailTemplateId = Guid.NewGuid();
        }

        [Test]
        public void ShouldRemoveEmailTemplateCorrectly()
        {
            _accessValidatorMock
                .Setup(x => x.HasRights(Rights.AddEditRemoveEmailTemplates))
                .Returns(true);

            _repositoryMock
                .Setup(x => x.DisableEmailTemplate(It.IsAny<Guid>()))
                .Returns(true);

            var expectedResponse = new OperationResultResponse<bool>
            {
                Status = OperationResultStatusType.FullSuccess,
                Body = true
            };

            SerializerAssert.AreEqual(expectedResponse, _command.Execute(_emailTemplateId));

            _repositoryMock.Verify();
        }

        [Test]
        public void ShouldThrowExceptionWhenRepositoryThrowsIt()
        {
            _repositoryMock.Setup(x => x.DisableEmailTemplate(It.IsAny<Guid>())).Throws(new Exception());

            Assert.Throws<Exception>(() => _command.Execute(_emailTemplateId));
            _repositoryMock.Verify();
        }

        [Test]
        public void ShouldThrowExceptionWhenUserHasNoRight()
        {
            _accessValidatorMock
                .Setup(x => x.HasRights(Rights.AddEditRemoveEmailTemplates))
                .Returns(false);

            Assert.Throws<ForbiddenException>(() => _command.Execute(_emailTemplateId));
        }
    }
}