using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Data.Provider;
using LT.DigitalOffice.MessageService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.MessageService.Models.Db;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.MessageService.Data.UnitTests
{
    public class EmailTemplateRepositoryTests
    {
        private IDataProvider provider;
        private IEmailTemplateRepository repository;

        private DbEmailTemplate dbEmailTemplate;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var dbOptions = new DbContextOptionsBuilder<MessageServiceDbContext>()
                   .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                   .Options;
            provider = new MessageServiceDbContext(dbOptions);

            repository = new EmailTemplateRepository(provider);
        }

        [SetUp]
        public void SetUp()
        {
            dbEmailTemplate = new DbEmailTemplate
            {
                Id = Guid.NewGuid(),
                Subject = "Subject",
                Body = "Body",
                AuthorId = Guid.NewGuid(),
                IsActive = true,
                CreatedAt = DateTime.Now
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

        [Test]
        public void ShouldAddEmailTemplateCorrectly()
        {
            Assert.AreEqual(dbEmailTemplate.Id, repository.AddEmailTemplate(dbEmailTemplate));
            Assert.AreEqual(dbEmailTemplate, provider.EmailTemplates.Find(dbEmailTemplate.Id));
        }
    }
}

