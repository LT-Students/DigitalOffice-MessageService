using LT.DigitalOffice.MessageService.Data.Interfaces;
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
    class WorkspaceUserRepositoryTests
    {
        /*private IDataProvider _provider;
        private IWorkspaceUserRepository _repository;

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

            _repository = new WorkspaceUserRepository(_provider);
        }

        [TearDown]
        public void CleanDb()
        {
            if (_provider.IsInMemory())
            {
                _provider.EnsureDeleted();
            }
        }

        #region GetAdmins Tests

        [Test]
        public void ShouldCorrectlyReturnAdminListOfWorkspace()
        {
            var _dbWorkspaceAdmin = new DbWorkspaceUser
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                WorkspaceId = Guid.NewGuid(),
                IsAdmin = true,
                IsActive = true
            };

            _provider.WorkspaceUsers.Add(_dbWorkspaceAdmin);
            _provider.Save();

            var expected = new List<DbWorkspaceUser> { _dbWorkspaceAdmin };

            SerializerAssert.AreEqual(expected, _repository.GetAdminsAsync(_dbWorkspaceAdmin.WorkspaceId));
        }

        #endregion

        #region AddRange Tests

        [Test]
        public void ShouldSuccessfulyAddRangeOfUsers()
        {
            Guid workspaceId = Guid.NewGuid();

            List<DbWorkspaceUser> users = new()
            {
                new DbWorkspaceUser
                {
                    Id = Guid.NewGuid(),
                    WorkspaceId = workspaceId
                },
                new DbWorkspaceUser
                {
                    Id = Guid.NewGuid(),
                    WorkspaceId = workspaceId
                },
                new DbWorkspaceUser
                {
                    Id = Guid.NewGuid(),
                    WorkspaceId = workspaceId
                }
            };

            _repository.CreateAsync(users);

            SerializerAssert.AreEqual(users, _provider.WorkspaceUsers.ToList());
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWhenCollectionIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _repository.CreateAsync(null));
        }

        #endregion*/
    }
}
