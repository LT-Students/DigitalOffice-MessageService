using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Data.Provider;
using LT.DigitalOffice.MessageService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.MessageService.Models.Db;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.MessageService.Data.UnitTests
{
    public class EmailRepositoryTests
    {
        private IDataProvider provider;
        private IEmailRepository repository;

        private DbEmail emailToSave;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var dbOptions = new DbContextOptionsBuilder<MessageServiceDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            provider = new MessageServiceDbContext(dbOptions);

            repository = new EmailRepository(provider);
        }

        [SetUp]
        public void SetUp()
        {
            emailToSave = new DbEmail
            {
                Id = Guid.NewGuid(),
                SenderId = Guid.NewGuid(),
                Receiver = "lalagvanan@gmail.com",
                Subject = "Subject",
                Body = "Body"
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
        public void ShouldSaveEmailCorrectly()
        {
            repository.SaveEmail(emailToSave);

            Assert.That(provider.Emails.Find(emailToSave.Id), Is.EqualTo(emailToSave));
        }
        #endregion
    }
}
