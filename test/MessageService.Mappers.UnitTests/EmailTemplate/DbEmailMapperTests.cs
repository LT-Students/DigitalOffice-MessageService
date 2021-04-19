using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.MessageService.Mappers.Db;
using LT.DigitalOffice.MessageService.Mappers.Db.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.UnitTestKernel;
using Moq;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.MessageService.Mappers.UnitTests.EmailTemplate
{
    public class DbEmailMapperTests
    {
        private IDbEmailMapper _mapper;
        private Mock<ISendEmailRequest> _emailMock;

        private DbEmail _dbEmail;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _emailMock = new Mock<ISendEmailRequest>();

            var senderId = Guid.NewGuid();
            var receiver = "lalagvana@gmail.com";

            _dbEmail = new DbEmail
            {
                SenderId = senderId,
                Receiver = "lalagvana@gmail.com"
            };

            _emailMock
                .Setup(x => x.SenderId)
                .Returns(senderId);
            _emailMock
                .Setup(x => x.Email)
                .Returns(receiver);

            _mapper = new DbEmailMapper();
        }

        #region ISendEmailRequestToDbEmail
        [Test]
        public void ShouldReturnDbEmailCorrectly()
        {
           var result = _mapper.Map(_emailMock.Object);

            _dbEmail.Id = result.Id;
            _dbEmail.Time = result.Time;

           SerializerAssert.AreEqual(_dbEmail, result);
        }

        [Test]
        public void ShouldThrowExceptionIfEmailIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _mapper.Map(null));
        }
        #endregion
    }
}
