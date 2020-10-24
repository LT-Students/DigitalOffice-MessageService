using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Data.Provider;
using LT.DigitalOffice.MessageService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.MessageService.Models.Db;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.MessageService.Data.UnitTests
{
    public class WorkspaceRepositoryTests
    {
        private IDataProvider provider;
        private IWorkspaceRepository repository;

        private DbWorkspace dbWorkspaceToAdd;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var dbOptions = new DbContextOptionsBuilder<MessageServiceDbContext>()
                   .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                   .Options;
            provider = new MessageServiceDbContext(dbOptions);

            repository = new WorkspaceRepository(provider);
        }

        [SetUp]
        public void SetUp()
        {
            dbWorkspaceToAdd = new DbWorkspace
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                IsActive = true,
            };
        }

        [TearDown]
        public void CleanDb()
        {
            if (provider.IsInMemory())
            {
                provider.EnsureDeleted();
            }
        }

        #region AddWorkspace
        [Test]
        public void ShouldAddEmailTemplateCorrectly()
        {
            var result = repository.AddWorkspace(dbWorkspaceToAdd);

            Assert.AreEqual(dbWorkspaceToAdd.Id, result);
            Assert.AreEqual(dbWorkspaceToAdd, provider.Workspaces.Find(dbWorkspaceToAdd.Id));
        }
        #endregion
    }
}
