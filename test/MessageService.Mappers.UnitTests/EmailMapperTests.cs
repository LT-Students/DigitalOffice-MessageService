using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.MessageService.Mappers.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.UnitTests.Models;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.UnitTestKernel;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.MessageService.Mappers.UnitTests
{
    public class EmailMapperTests
    {
        private IMapper<ISendEmailRequest, DbEmail> mapper;
        private ISendEmailRequest email;

        [SetUp]
        public void SetUp()
        {
            mapper = new EmailMapper();
        }

        #region ISendEmailRequestToDbEmail
        [Test]
        public void ShouldReturnDbEmailCorrectly()
        {
            email = new SendEmailRequest
            {
                Receiver = "lalagvana@gmail.com",
                Subject = "Subject",
                Body = "Body"
            };

            var result = mapper.Map(email);

            var dbEmail = new DbEmail
            {
                Id = result.Id,
                Receiver = email.Receiver,
                Time = result.Time,
                Subject = email.Subject,
                Body = email.Body
            };

            SerializerAssert.AreEqual(dbEmail, result);
        }

        [Test]
        public void ShouldThrowExceptionIfEmailIsNull()
        {
            email = null;

            Assert.Throws<ArgumentNullException>(() => mapper.Map(null));
        }
        #endregion
    }
}
