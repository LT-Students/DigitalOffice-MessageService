using LT.DigitalOffice.MessageService.Mappers.Db;
using LT.DigitalOffice.MessageService.Mappers.Db.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.Models.Broker.Requests.Message;
using LT.DigitalOffice.UnitTestKernel;
using Moq;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.MessageService.Mappers.UnitTests
{
    public class DbSMTPCredentialMapperTests
    {
        private IDbSMTPCredentialsMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            _mapper = new DbSMTPCredentialsMapper();
        }

        [Test]
        public void ShouldThrowNullArgumentExceptionWhenRequestIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _mapper.Map(null));
        }

        [Test]
        public void ShouldMapSuccessfully()
        {
            string host = "host";
            int port = 123;
            bool enableSsl = true;
            string email = "email";
            string password = "password";

            var request = new Mock<ICreateSMTPRequest>();
            request
                .Setup(x => x.Port)
                .Returns(port);
            request
                .Setup(x => x.Host)
                .Returns(host);
            request
                .Setup(x => x.Email)
                .Returns(email);
            request
                .Setup(x => x.Password)
                .Returns(password);
            request
                .Setup(x => x.EnableSsl)
                .Returns(enableSsl);

            var result = _mapper.Map(request.Object);

            var expected = new DbSMTPCredentials
            {
                Id = result.Id,
                Email = email,
                EnableSsl = enableSsl,
                Host = host,
                Password = password,
                Port = port
            };

            SerializerAssert.AreEqual(expected, result);
        }
    }
}
