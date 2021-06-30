using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.MessageService.Business.Commands.UnsentEmail;
using LT.DigitalOffice.MessageService.Business.Commands.UnsentEmail.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
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

        [Test]
        public void ShouldThrowExceptionWhenRepositoryThrow()
        {
            _mocker
                .Setup<IUnsentEmailRepository, DbUnsentEmail>(x => x.Get(It.IsAny<Guid>()))
                .Throws(new Exception());

            var id = Guid.NewGuid();

            Assert.Throws<Exception>(() => _command.Execute(id));

            _mocker
                .Verify<IAccessValidator, bool>(x => x.IsAdmin(null), Times.Once);
            _mocker
                .Verify<IUnsentEmailRepository, DbUnsentEmail>(x => x.Get(id), Times.Once);
        }

        //[Test]
        //public void ShouldResendEmailSuccessfuly()
        //{
        //    Guid emailId = Guid.NewGuid();

        //    DbUnsentEmail email = new()
        //    {
        //        Id = Guid.NewGuid(),
        //        CreatedAt = DateTime.UtcNow,
        //        LastSendAt = DateTime.UtcNow,
        //        TotalSendingCount = 1,
        //        Email = new()
        //        {
        //            Id = emailId,
        //            Body = "Body",
        //            Subject = "Subject",
        //            Receiver = "malkinevgeniy11@gmail.com",
        //            Time = DateTime.UtcNow
        //        },
        //        EmailId = emailId
        //    };

        //    _mocker
        //        .Setup<IUnsentEmailRepository, DbUnsentEmail>(x => x.Get(email.Id))
        //        .Returns(email);

        //    OperationResultResponse<bool> response = _command.Execute(email.Id);

        //    Assert.AreEqual(OperationResultStatusType.FullSuccess, response.Status);
        //    Assert.IsTrue(response.Body);
        //    _mocker
        //        .Verify<IAccessValidator, bool>(x => x.IsAdmin(null), Times.Once);
        //    _mocker
        //        .Verify<IUnsentEmailRepository, DbUnsentEmail>(x => x.Get(email.Id), Times.Once);
        //}
    }
}
