using LT.DigitalOffice.MessageService.Data.Provider;
using LT.DigitalOffice.MessageService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.UnitTestKernel;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.MessageService.Data.UnitTests
{
    public class ChannelUserRepositoryTests
    {
        private ChannelUserRepository _repository;
        private IDataProvider _provider;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            CreateMemoryDb();
        }

        public void CreateMemoryDb()
        {
            var dbOptions = new DbContextOptionsBuilder<MessageServiceDbContext>()
                   .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                   .Options;
            _provider = new MessageServiceDbContext(dbOptions);

            _repository = new ChannelUserRepository(_provider);
        }

        [TearDown]
        public void CleanDb()
        {
            if (_provider.IsInMemory())
            {
                _provider.EnsureDeleted();
            }
        }

        #region AddRange Tests

        [Test]
        public void ShouldAddRangeSuccessfuly()
        {
            Guid channelId = Guid.NewGuid();

            List<DbChannelUser> users = new()
            {
                new DbChannelUser
                {
                    Id = Guid.NewGuid(),
                    WorkspaceUserId = Guid.NewGuid(),
                    ChannelId = channelId
                },
                new DbChannelUser
                {
                    Id = Guid.NewGuid(),
                    WorkspaceUserId = Guid.NewGuid(),
                    ChannelId = channelId
                },
                new DbChannelUser
                {
                    Id = Guid.NewGuid(),
                    WorkspaceUserId = Guid.NewGuid(),
                    ChannelId = channelId
                }
            };

            _repository.AddRange(users);

            SerializerAssert.AreEqual(users, _provider.ChannelUsers.ToList());
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWhenAddedCollectionIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _repository.AddRange(null));
        }

        #endregion
    }
}
