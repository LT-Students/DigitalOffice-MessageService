using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Data.Provider;
using LT.DigitalOffice.MessageService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.MessageService.Models.Db;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Linq;

namespace LT.DigitalOffice.MessageService.Data.UnitTests
{
    class UserRepositoryTests
    {
        private IDataProvider _provider;
        private IUserRepository _repository;

        private DbWorkspaceAdmin _dbWorkspaceAdmin;

        private Guid _workspaceId = Guid.NewGuid();
        private Guid _userId = Guid.NewGuid();

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _dbWorkspaceAdmin = new DbWorkspaceAdmin
            {
                Id = Guid.NewGuid(),
                UserId = _userId,
                WorkspaceId = _workspaceId
            };

            CreateMemoryDb();
        }

        public void CreateMemoryDb()
        {
            var dbOptions = new DbContextOptionsBuilder<MessageServiceDbContext>()
                   .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                   .Options;
            _provider = new MessageServiceDbContext(dbOptions);

            _repository = new UserRepository(_provider);
        }

        [SetUp]
        public void SetUp()
        {
            _provider.WorkspaceAdmins.Add(_dbWorkspaceAdmin);
            _provider.Save();
        }

        [TearDown]
        public void CleanDb()
        {
            if (_provider.IsInMemory())
            {
                _provider.EnsureDeleted();
            }
        }

        [Test]
        public void ShouldCorrectlyReturnAdminListOfWorkspace()
        {
            var coll = _repository.GetAdmins(_workspaceId);
            Assert.IsNotNull(_repository.GetAdmins(_workspaceId).FirstOrDefault(a => a.UserId == _userId));
        }
    }
}
