using LT.DigitalOffice.MessageService.Mappers.Db;
using LT.DigitalOffice.MessageService.Mappers.Db.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.UnitTestKernel;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.MessageService.Mappers.UnitTests.Workspace
{
    public class DbWorkspaceUserMapperTests
    {
        private IDbWorkspaceUserMapper _mapper;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _mapper = new DbWorkspaceUserMapper();
        }

        [Test]
        public void ShouldMapSuccessfuly()
        {
            bool isAdmin = true;

            DbWorkspaceUser user = new DbWorkspaceUser
            {
                UserId = Guid.NewGuid(),
                WorkspaceId = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = isAdmin
            };

            var result = _mapper.Map(user.WorkspaceId, user.UserId, isAdmin);

            user.Id = result.Id;
            user.CreatedAt = result.CreatedAt;

            SerializerAssert.AreEqual(user, result);
        }
    }
}
