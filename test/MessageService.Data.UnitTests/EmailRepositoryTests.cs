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
        private IDataProvider _provider;
        private IEmailRepository _repository;

        private DbEmail _emailToSave;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var dbOptions = new DbContextOptionsBuilder<MessageServiceDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            _provider = new MessageServiceDbContext(dbOptions);

            _repository = new EmailRepository(_provider);
        }

        [SetUp]
        public void SetUp()
        {
            _emailToSave = new DbEmail
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
            if (_provider.IsInMemory())
            {
                _provider.EnsureDeleted();
            }
        }

        #region SaveMessage
        [Test]
        public void ShouldSaveEmailCorrectly()
        {
            _repository.SaveEmail(_emailToSave);

            Assert.That(_provider.Emails.Find(_emailToSave.Id), Is.EqualTo(_emailToSave));
        }
        #endregion
    }
}
