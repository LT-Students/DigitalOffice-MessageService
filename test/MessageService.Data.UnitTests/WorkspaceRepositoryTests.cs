using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Data.Provider;
using LT.DigitalOffice.MessageService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.MessageService.Models.Db;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using LT.DigitalOffice.UnitTestKernel;
using System.Linq;
using LT.DigitalOffice.Kernel.Exceptions;

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
                Name = "Name",
                IsActive = true
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

        #region GetWorkspace
        [Test]
        public void ShouldReturnExistsWorkspace()
        {
            SerializerAssert.AreEqual(_repository.GetWorkspace(_dbWorkspaceInDb.Id), _dbWorkspaceInDb);
        }

        [Test]
        public void ShouldThrowNotFountExcWhenDoesNotExistWorkspaceWithThisId()
        {
            var incorrectId = Guid.NewGuid();

            Assert.Throws<NotFoundException>(() => _repository.GetWorkspace(incorrectId));
        }
        #endregion

        #region SwitchActiveStatus
        [Test]
        public void ShouldSwitchActiveStatusSuccessfully()
        {
            _dbWorkspaceInDb.IsActive = true;

            var id = _dbWorkspaceInDb.Id;

            Assert.IsTrue(_repository.SwitchActiveStatus(id, false));
            Assert.IsFalse(_provider.Workspaces.FirstOrDefault(w => w.Id == id).IsActive);
        }

        [Test]
        public void ShouldThrowNotFountExcWhenTryingSwitchStatusOfNonExistsWorkspace()
        {
            var id = Guid.NewGuid();

            Assert.Throws<NotFoundException>(() => _repository.SwitchActiveStatus(id, false));
        }
        #endregion
    }
}
