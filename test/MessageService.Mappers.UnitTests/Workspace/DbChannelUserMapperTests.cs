using LT.DigitalOffice.MessageService.Mappers.Db;
using LT.DigitalOffice.MessageService.Mappers.Db.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.UnitTestKernel;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.MessageService.Mappers.UnitTests.Workspace
{
    public class DbChannelUserMapperTests
    {
        private IDbChannelUserMapper _mapper;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _mapper = new DbChannelUserMapper();
        }

        [Test]
        public void ShouldMapSuccessfuly()
        {
            bool isAdmin = true;

            DbChannelUser user = new()
            {
                WorkspaceUserId = Guid.NewGuid(),
                ChannelId = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = isAdmin
            };

            var result = _mapper.Map(user.ChannelId, user.WorkspaceUserId, isAdmin);

            user.Id = result.Id;
            user.CreatedAt = result.CreatedAt;

            SerializerAssert.AreEqual(user, result);
        }
    }
}
