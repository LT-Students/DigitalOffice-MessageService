using LT.DigitalOffice.Kernel.UnitTestLibrary;
using LT.DigitalOffice.MessageService.Mappers.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.MessageService.Mappers.UnitTests
{
    public class MessageMapperTests
    {
        private IMapper<Message, DbMessage> mapper;

        [SetUp]
        public void SetUp()
        {
            mapper = new MessageMapper();
        }

        #region MessageToDbMessage
        [Test]
        public void ShouldReturnDbMessageCorrectly()
        {
            var message = new Message
            {
                Title = "Title",
                Content = "Content"
            };

            var result = mapper.Map(message);

            var dbMessage = new DbMessage
            {
                Id = result.Id,
                Title = "Title",
                Content = "Content",
                Status = 0
            };

            SerializerAssert.AreEqual(dbMessage, result);
        }

        [Test]
        public void ShouldThrowExceptionIfMessageIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => mapper.Map(null));
        }
        #endregion
    }
}
