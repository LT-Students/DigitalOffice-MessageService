using LT.DigitalOffice.MessageService.Mappers.Models;
using LT.DigitalOffice.MessageService.Mappers.Models.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Models.Emails;
using LT.DigitalOffice.UnitTestKernel;
using Moq;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.MessageService.Mappers.UnitTests.UnsentEmail
{
    public class UnsentEmailInfoMapperTests
    {
        private IUnsentEmailInfoMapper _mapper;
        private Mock<IEmailInfoMapper> _emailMapperMock;

        private DbUnsentEmail _dbUnsentEmail;
        private DbEmail _dbEmail;
        private EmailInfo _emailInfo;

        [SetUp] 
        public void SetUp()
        {
            _emailMapperMock = new Mock<IEmailInfoMapper>();

            _dbEmail = new DbEmail
            {
                Id = Guid.NewGuid(),
                Body = "Body",
                Subject = "Subject",
                Receiver = "Receiver"
            };

            _emailInfo = new EmailInfo
            {
                Id = _dbEmail.Id,
                Body = _dbEmail.Body,
                Subject = _dbEmail.Subject,
                Receiver = _dbEmail.Receiver
            };

            _dbUnsentEmail = new DbUnsentEmail
            {
                Id = Guid.NewGuid(),
                EmailId = _dbEmail.Id,
                CreatedAtUtc = DateTime.UtcNow,
                LastSendAtUtc = DateTime.UtcNow,
                TotalSendingCount = 2,
                Email = _dbEmail
            };

            _emailMapperMock
                .Setup(x => x.Map(null))
                .Throws(new ArgumentNullException());

            _emailMapperMock
                .Setup(x => x.Map(_dbEmail))
                .Returns(_emailInfo);

            _mapper = new UnsentEmailInfoMapper(_emailMapperMock.Object);
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWhenDbEmailIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _mapper.Map(null));
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWhenEmailIsNull()
        {
            _dbUnsentEmail.Email = null;

            Assert.Throws<ArgumentNullException>(() => _mapper.Map(_dbUnsentEmail));
        }

        [Test]
        public void ShouldMapSuccessful()
        {
            var expectedResult = new UnsentEmailInfo
            {
                Id = _dbUnsentEmail.Id,
                Email = _emailInfo,
                LastSendAt = _dbUnsentEmail.LastSendAtUtc,
                CreatedAt = _dbUnsentEmail.CreatedAtUtc,
                TotalSendingCount = _dbUnsentEmail.TotalSendingCount
            };

            SerializerAssert.AreEqual(expectedResult, _mapper.Map(_dbUnsentEmail));
        }
    }
}
