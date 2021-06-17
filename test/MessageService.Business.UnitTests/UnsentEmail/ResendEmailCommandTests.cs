using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.MessageService.Business.UnsentEmailCommands;
using LT.DigitalOffice.MessageService.Business.UnsentEmailCommands.Interfaces;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.MessageService.Business.UnitTests.UnsentEmail
{
    public class ResendEmailCommandTests
    {
        private AutoMocker _mocker;
        private IResendEmailCommand _command;

        [SetUp]
        public void SetUp()
        {
            _mocker = new();
            _command = _mocker.CreateInstance<ResendEmailCommand>();

            _mocker
                .Setup<IAccessValidator, bool>(x => x.IsAdmin(null))
                .Returns(true);
        }

        [Test]
        public void ShouldThrowForbiddenExceptionWhenUserIsNotAdmin()
        {
            _mocker
                .Setup<IAccessValidator, bool>(x => x.IsAdmin(null))
                .Returns(false);

            Assert.Throws<ForbiddenException>(() => _command.Execute(Guid.NewGuid()));
            _mocker
                .Verify<IAccessValidator, bool>(x => x.IsAdmin(null), Times.Once);
        }
    }
}
