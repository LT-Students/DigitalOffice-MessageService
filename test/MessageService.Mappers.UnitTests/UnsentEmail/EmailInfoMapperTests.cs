using LT.DigitalOffice.MessageService.Mappers.Models;
using LT.DigitalOffice.MessageService.Mappers.Models.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Models.Emails;
using LT.DigitalOffice.UnitTestKernel;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.MessageService.Mappers.UnitTests.UnsentEmail
{
    public class EmailInfoMapperTests
    {
        private IEmailInfoMapper _mapper;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _mapper = new EmailInfoMapper();
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWhenEntityIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _mapper.Map(null));
        }

        [Test]
        public void ShouldMapSuccessful()
        {
            var email = new DbEmail
            {
                Id = Guid.NewGuid(),
                Body = "Body",
                Subject = "Subject",
                Receiver = "receiver"
            };

            var expected = new EmailInfo
            {
                Id = email.Id,
                Body = email.Body,
                Subject = email.Subject,
                To = email.Receiver
            };

            SerializerAssert.AreEqual(expected, _mapper.Map(email));
        }
    }
}
