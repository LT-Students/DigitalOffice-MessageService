using LT.DigitalOffice.Kernel.UnitTestLibrary;
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
        private DbEmailTemplate editDbEmailTemplate;

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

            editDbEmailTemplate = new DbEmailTemplate
            {
                Id = dbEmailTemplate.Id,
                Subject = "Subject_1",
                Body = "Body_1",
                AuthorId = dbEmailTemplate.AuthorId,
                IsActive = dbEmailTemplate.IsActive,
                CreatedAt = dbEmailTemplate.CreatedAt
            };

            provider.EmailTemplates.Add(dbEmailTemplate);
            provider.Save();
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
            var newdbEmailTemplate = new DbEmailTemplate
            {
                Id = Guid.NewGuid(),
                Subject = "Subject",
                Body = "Body",
                AuthorId = Guid.NewGuid(),
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            Assert.AreEqual(newdbEmailTemplate.Id, repository.AddEmailTemplate(newdbEmailTemplate));
            Assert.AreEqual(newdbEmailTemplate, provider.EmailTemplates.Find(newdbEmailTemplate.Id));
        }

        [Test]
        public void ShouldThrowNullReferenceExceptionWhenEmailTemplateIdNotFound()
        {
            var emailTemplaiId = Guid.NewGuid();

            Assert.Throws<NullReferenceException>(() => repository.GetEmailTemplateById(emailTemplaiId));
        }

        [Test]
        public void ShouldGetEmailTemplateByIdSuccessful()
        {
            SerializerAssert.AreEqual(dbEmailTemplate, repository.GetEmailTemplateById(dbEmailTemplate.Id));
        }

        [Test]
        public void ShouldThrowNullReferenceExceptionWhenEditEmailTemplateIdNotFound()
        {
            var newdbEmailTemplate = new DbEmailTemplate
            {
                Id = Guid.NewGuid(),
                Subject = editDbEmailTemplate.Subject,
                Body = editDbEmailTemplate.Body,
                AuthorId = editDbEmailTemplate.AuthorId,
                IsActive = editDbEmailTemplate.IsActive,
                CreatedAt = editDbEmailTemplate.CreatedAt
            };

            Assert.Throws<NullReferenceException>(() => repository.EditEmailTemplate(newdbEmailTemplate));
        }

        [Test]
        public void ShouldEditEmailTemplateSuccessful()
        {
            provider.MakeEntityDetached(dbEmailTemplate);

            repository.EditEmailTemplate(editDbEmailTemplate);

            SerializerAssert.AreEqual(editDbEmailTemplate, repository.GetEmailTemplateById(editDbEmailTemplate.Id));
        }
    }
}

