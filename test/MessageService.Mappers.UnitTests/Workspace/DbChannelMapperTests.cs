using LT.DigitalOffice.MessageService.Mappers.Db;
using LT.DigitalOffice.MessageService.Mappers.Db.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.UnitTestKernel;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.MessageService.Mappers.UnitTests.Workspace
{
    public class DbChannelMapperTests
    {
        private IDbChannelMapper _mapper;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _mapper = new DbChannelMapper();
        }

        [Test]
        public void ShouldMapSuccessfuly()
        {
            bool isPrivate = true;
            string name = "General";

            DbChannel channel = new()
            {
                WorkspaceId = Guid.NewGuid(),
                OwnerId = Guid.NewGuid(),
                Name = name,
                IsActive = true,
                IsPrivate = isPrivate
            };

            var result = _mapper.Map(channel.WorkspaceId, channel.OwnerId, name, isPrivate);

            channel.Id = result.Id;
            channel.CreatedAt = result.CreatedAt;

            SerializerAssert.AreEqual(channel, result);
        }
    }
}
