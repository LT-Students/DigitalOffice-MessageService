using LT.DigitalOffice.Kernel.Exceptions.Models;
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
using System.Text;
using System.Threading.Tasks;

namespace LT.DigitalOffice.MessageService.Data.UnitTests
{
    public class SMTPCredentialRepositoryTests
    {
        private IDataProvider _provider;
        private ISMTPCredentialsRepository _repository;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var dbOptions = new DbContextOptionsBuilder<MessageServiceDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            _provider = new MessageServiceDbContext(dbOptions);

            _repository = new SMTPCredentialsRepository(_provider);
        }

        [TearDown]
        public void CleanDb()
        {
            if (_provider.IsInMemory())
            {
                _provider.EnsureDeleted();
            }
        }

        #region Create tests

        [Test]
        public void ShouldCreateSMTPSuccessfully()
        {
            DbSMTPCredentials smtp = new()
            {
                Id = Guid.NewGuid()
            };

            _repository.Create(smtp);

            Assert.IsTrue(_provider.SMTP.Contains(smtp));
        }

        [Test]
        public void ShouldThrowBadRequestExceptionWhenSMTPExists()
        {
            DbSMTPCredentials smtp1 = new()
            {
                Id = Guid.NewGuid()
            };

            DbSMTPCredentials smtp2 = new()
            {
                Id = Guid.NewGuid()
            };

            _repository.Create(smtp1);

            Assert.Throws<BadRequestException>(() => _repository.Create(smtp1));
        }

        #endregion

        #region Get tests

        [Test]
        public void ShouldGetSMTPSuccessfully()
        {
            DbSMTPCredentials smtp = new()
            {
                Id = Guid.NewGuid(),
                Email = "email",
                EnableSsl = true,
                Host = "host",
                Password = "password",
                Port = 123
            };

            _provider.SMTP.Add(smtp);
            _provider.Save();

            SerializerAssert.AreEqual(smtp, _repository.Get());
        }

        [Test]
        public void ShouldThrowNotFoundExceptionWhenSMTPDoesNotExist()
        {
            Assert.Throws<NotFoundException>(() => _repository.Get());
        }

        #endregion
    }
}
