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
    public class UnsentEmailRepositoryTests
    {
        private IUnsentEmailRepository _repository;
        private IDataProvider _provider;

        private DbEmail _emailInDb1;
        private DbEmail _emailInDb2;

        private DbUnsentEmail _unsentEmailInDb1;
        private DbUnsentEmail _unsentEmailInDb2;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var dbOptions = new DbContextOptionsBuilder<MessageServiceDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            _provider = new MessageServiceDbContext(dbOptions);

            _emailInDb1 = new DbEmail
            {
                Id = Guid.NewGuid()
            };

            _emailInDb2 = new DbEmail
            {
                Id = Guid.NewGuid()
            };

            _unsentEmailInDb1 = new DbUnsentEmail
            {
                Id = Guid.NewGuid(),
                EmailId = _emailInDb1.Id,
                CreatedAt = DateTime.UtcNow,
                LastSendAt = DateTime.UtcNow,
                TotalSendingCount = 2
            };
            _unsentEmailInDb2 = new DbUnsentEmail
            {
                Id = Guid.NewGuid(),
                EmailId = _emailInDb2.Id,
                CreatedAt = DateTime.UtcNow,
                LastSendAt = DateTime.UtcNow,
                TotalSendingCount = 3
            };

            _provider.Emails.Add(_emailInDb1);
            _provider.Emails.Add(_emailInDb2);
            _provider.UnsentEmails.Add(_unsentEmailInDb2);
            _provider.UnsentEmails.Add(_unsentEmailInDb1);
            _provider.Save();

            _repository = new UnsentEmailRepository(_provider);
        }

        [SetUp]
        public void SetUp()
        {
            
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
        public void ShouldGetAllUnsentEmails()
        {
            var response = _repository.GetAll();

            foreach (var email in response)
            {
                var emailInDb = _provider.UnsentEmails.FirstOrDefault(ue => ue.Id == email.Id);
                Assert.AreEqual(emailInDb.Id, email.Id);
                Assert.AreEqual(emailInDb.LastSendAt, email.LastSendAt);
                Assert.AreEqual(emailInDb.CreatedAt, email.CreatedAt);
                Assert.AreEqual(emailInDb.EmailId, email.EmailId);
                Assert.AreEqual(emailInDb.TotalSendingCount, email.TotalSendingCount);
            }

            Assert.AreEqual(_provider.UnsentEmails.ToList().Count, response.Count());
        }
    }
}
