using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.MessageService.Business.Commands.UnsentEmail;
using LT.DigitalOffice.MessageService.Business.Commands.UnsentEmail.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Models.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Models.Emails;
using LT.DigitalOffice.MessageService.Models.Dto.Responses;
using LT.DigitalOffice.UnitTestKernel;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.MessageService.Business.UnitTests.UnsentEmail
{
    public class FindUnsentEmailsCommandTests
    {
        private AutoMocker _mocker;
        private IFindUnsentEmailsCommand _command;

        [SetUp]
        public void SetUp()
        {
            _mocker = new AutoMocker();
            _command = _mocker.CreateInstance<FindUnsentEmailsCommand>();

            _mocker
                .Setup<IAccessValidator, bool>(x => x.IsAdmin(null))
                .Returns(true);
        }

        [Test]
        public void ShouldThrowForbiddenExceptionWhenUserIsNotAdmin()
        {
            int total = 0;

            _mocker
                .Setup<IAccessValidator, bool>(x => x.IsAdmin(null))
                .Returns(false);

            Assert.Throws<ForbiddenException>(() => _command.Execute(1, 2));
            _mocker
                .Verify<IAccessValidator, bool>(x => x.IsAdmin(null), Times.Once);
            _mocker
                .Verify<IUnsentEmailRepository, IEnumerable<DbUnsentEmail>>(x => x.Find(It.IsAny<int>(), It.IsAny<int>(), out total), Times.Never);
            _mocker
                .Verify<IUnsentEmailInfoMapper, UnsentEmailInfo>(x => x.Map(It.IsAny<DbUnsentEmail>()), Times.Never);
        }

        [Test]
        public void ShouldThrowExceptionWhenRepositoryThrow()
        {
            int total = 0;
            int skip = 1;
            int take = 1;

            _mocker
                .Setup<IUnsentEmailRepository, IEnumerable<DbUnsentEmail>>(x => x.Find(It.IsAny<int>(), It.IsAny<int>(), out total))
                .Throws(new Exception());

            Assert.Throws<Exception>(() => _command.Execute(skip, take));
            _mocker
                .Verify<IAccessValidator, bool>(x => x.IsAdmin(null), Times.Once);
            _mocker
                .Verify<IUnsentEmailRepository, IEnumerable<DbUnsentEmail>>(x => x.Find(skip, take, out total), Times.Once);
            _mocker
                .Verify<IUnsentEmailInfoMapper, UnsentEmailInfo>(x => x.Map(It.IsAny<DbUnsentEmail>()), Times.Never);
        }

        [Test]
        public void ShouldSuccessfulGetAllUnsentEmails()
        {
            int total = 3;
            int skip = 0;
            int take = 2;

            List<DbUnsentEmail> emails = new List<DbUnsentEmail>
            {
                new DbUnsentEmail
                {
                    Id = Guid.NewGuid(),
                    EmailId = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    LastSendAt = DateTime.UtcNow,
                    TotalSendingCount = 2
                },
                new DbUnsentEmail
                {
                    Id = Guid.NewGuid(),
                    EmailId = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    LastSendAt = DateTime.UtcNow,
                    TotalSendingCount = 2
                }
            };

            UnsentEmailInfo info1 = new UnsentEmailInfo
            {
                Id = emails[0].Id,
                CreatedAt = emails[0].CreatedAt,
                LastSendAt = emails[0].LastSendAt,
                TotalSendingCount = emails[0].TotalSendingCount,
                Email = new EmailInfo
                {
                    Id = emails[0].EmailId,
                    Body = "Body",
                    Subject = "Subject",
                    Receiver = "To"
                }
            };

            UnsentEmailInfo info2 = new UnsentEmailInfo
            {
                Id = emails[0].Id,
                CreatedAt = emails[0].CreatedAt,
                LastSendAt = emails[0].LastSendAt,
                TotalSendingCount = emails[0].TotalSendingCount,
                Email = new EmailInfo
                {
                    Id = emails[0].EmailId,
                    Body = "Body",
                    Subject = "Subject",
                    Receiver = "To"
                }
            };

            _mocker
                .Setup<IUnsentEmailInfoMapper, UnsentEmailInfo>(x => x.Map(emails[0]))
                .Returns(info1);

            _mocker
                .Setup<IUnsentEmailInfoMapper, UnsentEmailInfo>(x => x.Map(emails[1]))
                .Returns(info2);

            UnsentEmailsResponse expected = new UnsentEmailsResponse
            {
                TotalCount = 3,
                Emails = new List<UnsentEmailInfo> { info1, info2 }
            };

            _mocker
                .Setup<IUnsentEmailRepository, IEnumerable<DbUnsentEmail>>(x => x.Find(It.IsAny<int>(), It.IsAny<int>(), out total))
                .Returns(emails);

            SerializerAssert.AreEqual(expected, _command.Execute(skip, take));
            _mocker
                .Verify<IAccessValidator, bool>(x => x.IsAdmin(null), Times.Once);
            _mocker
                .Verify<IUnsentEmailRepository, IEnumerable<DbUnsentEmail>>(x => x.Find(skip, take, out total), Times.Once);
            _mocker
                .Verify<IUnsentEmailInfoMapper, UnsentEmailInfo>(x => x.Map(It.IsAny<DbUnsentEmail>()), Times.Exactly(2));
        }
    }
}
