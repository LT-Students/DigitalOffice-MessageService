using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Data.Provider;
using LT.DigitalOffice.MessageService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.MessageService.Models.Db;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.MessageService.Data.UnitTests
{
    public class MessageRepositoryTests
    {
        private IDataProvider provider;
        private IMessageRepository repository;

        private DbMessage messageToSave;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var dbOptions = new DbContextOptionsBuilder<MessageServiceDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            provider = new MessageServiceDbContext(dbOptions);

            repository = new MessageRepository(provider);
        }

        [SetUp]
        public void SetUp()
        {
            messageToSave = new DbMessage
            {
                Id = Guid.NewGuid(),
                Title = "Title",
                Content = "Content",
                Status = 0
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

        #region SaveMessage
        [Test]
        public void ShouldSaveMessageCorrectly()
        {
            repository.SaveMessage(messageToSave);

            Assert.That(provider.Messages.Find(messageToSave.Id), Is.EqualTo(messageToSave));
        }
        #endregion
    }
}
