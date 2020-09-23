using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.Kernel.UnitTestLibrary;
using LT.DigitalOffice.MessageService.Mappers.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.MessageService.Mappers.UnitTests
{
    public class EmailMapperTests
    {
        private IMapper<ISendEmailRequest, DbEmail> mapper;

        [SetUp]
        public void SetUp()
        {
            mapper = new EmailMapper();
        }

        #region ISendEmailRequestToDbEmail
        /*[Test]
        public void ShouldReturnDbEmailCorrectly()
        {
            var email = new Email
            {
                Receiver = "lalagvanan@gmail.com",
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
        */
        [Test]
        public void ShouldThrowExceptionIfEmailIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => mapper.Map(null));
        }
        #endregion
    }
}
