using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Data.Provider;
using LT.DigitalOffice.MessageService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.Kernel.Exceptions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.MessageService.Data.UnitTests
{
    internal class WorkspaceRepositoryTests
    {
        private IDataProvider _provider;
        private IWorkspaceRepository _repository;

        private DbWorkspace _dbWorkspaceToAdd;
        private DbWorkspace _dbWorkspaceInDb;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _dbWorkspaceToAdd = new DbWorkspace
            {
                Name = "Name",
                Description = "Description",
                IsActive = true,
            };

            _dbWorkspaceInDb = new DbWorkspace
            {
                Id = Guid.NewGuid(),
                Name = "Name"
            };

            CreateMemoryDb();
        }

        public void CreateMemoryDb()
        {
            var dbOptions = new DbContextOptionsBuilder<MessageServiceDbContext>()
                   .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                   .Options;
            _provider = new MessageServiceDbContext(dbOptions);

            _repository = new WorkspaceRepository(_provider);
        }

        [SetUp]
        public void SetUp()
        {
            _provider.Workspaces.Add(_dbWorkspaceInDb);
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

        #region AddWorkspace
        [Test]
        public void ShouldAddEmailTemplateCorrectly()
        {
            _dbWorkspaceToAdd.Id = Guid.NewGuid();

            var result = _repository.CreateWorkspace(_dbWorkspaceToAdd);

            Assert.AreEqual(_dbWorkspaceToAdd.Id, result);
            Assert.AreEqual(_dbWorkspaceToAdd, _provider.Workspaces.Find(_dbWorkspaceToAdd.Id));
        }
        #endregion
    }
}
