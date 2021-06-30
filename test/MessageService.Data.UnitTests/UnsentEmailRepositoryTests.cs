using LT.DigitalOffice.Kernel.Exceptions.Models;
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

        [SetUp]
        public void SetUp()
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

        [Test]
        public void ShouldFindAllUnsentEmails()
        {
            var response1 = _repository.Find(0, 1, out int total).First();
            var response2 = _repository.Find(1, 1, out int _).First();

            Assert.AreEqual(_provider.UnsentEmails.ToList().Count, total);
            Assert.IsTrue(_provider.UnsentEmails.ToList().Contains(response1));
            Assert.IsTrue(_provider.UnsentEmails.ToList().Contains(response2));
            Assert.AreNotEqual(response1, response2);
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWhenEmailToAddIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _repository.Add(null));
        }

        [Test]
        public void ShouldAddUnsentEmailSuccessful()
        {
            DbUnsentEmail email = new DbUnsentEmail
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                LastSendAt = DateTime.UtcNow,
                EmailId = Guid.NewGuid(),
                TotalSendingCount = 1
            };

            _repository.Add(email);

            Assert.NotNull(_provider.UnsentEmails.ToList().Contains(email));
        }

        [Test]
        public void ShouldThrowNotFoundExceptionWhenEmailDoesNotExist()
        {
            Assert.Throws<NotFoundException>(() => _repository.Get(Guid.NewGuid()));
        }

        [Test]
        public void ShouldGetUnsentEmailSuccessful()
        {
            Assert.AreEqual(_unsentEmailInDb1, _repository.Get(_unsentEmailInDb1.Id));
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWhenEmailToRemoveIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _repository.Remove(null));
        }

        [Test]
        public void ShouldRemoveUnsentEmailSuccessful()
        {
            var emailForRemove = new DbUnsentEmail
            {
                Id = Guid.NewGuid(),
                EmailId = _emailInDb1.Id,
                CreatedAt = DateTime.UtcNow,
                LastSendAt = DateTime.UtcNow,
                TotalSendingCount = 2
            };

            _provider.UnsentEmails.Add(emailForRemove);
            _provider.Save();

            Assert.IsTrue(_repository.Remove(emailForRemove));
            Assert.IsFalse(_provider.UnsentEmails.ToList().Contains(emailForRemove));
        }

        [Test]
        public void ShouldIncrementTotalCountUnsentEmailSuccessful()
        {
            var count = _unsentEmailInDb1.TotalSendingCount;
            var time = _unsentEmailInDb1.LastSendAt;
            _repository.IncrementTotalCount(_unsentEmailInDb1);

            Assert.AreEqual(count + 1, _unsentEmailInDb1.TotalSendingCount);
            Assert.Less(time, _unsentEmailInDb1.LastSendAt);
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWhenEmailToIncrementIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _repository.IncrementTotalCount(null));
        }
    }
}
