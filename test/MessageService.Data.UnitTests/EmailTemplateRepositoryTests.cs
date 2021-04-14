using LT.DigitalOffice.Kernel.Exceptions;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Data.Provider;
using LT.DigitalOffice.MessageService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Enums;
using LT.DigitalOffice.UnitTestKernel;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.MessageService.Data.UnitTests
{
    public class EmailTemplateRepositoryTests
    {
        private IDataProvider provider;
        private IEmailTemplateRepository repository;

        private DbEmailTemplate dbEmailTemplate;
        private Guid emailTemplateId;
        private DbEmailTemplate dbEmailTemplateToAdd;
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
            emailTemplateId = Guid.NewGuid();

            var dbEmailTemplateText = new DbEmailTemplateText
            {
                Id = Guid.NewGuid(),
                EmailTemplateId = emailTemplateId,
                Subject = "Subject",
                Text = "Hello, {userFirstName}!!! Your password: {userPassword}, enter it at login",
                Language = "en"
            };

            dbEmailTemplateToAdd = new DbEmailTemplate
            {
                Id = Guid.NewGuid(),
                AuthorId = Guid.NewGuid(),
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                EmailTemplateTexts = new List<DbEmailTemplateText>
                {
                    dbEmailTemplateText
                }
            };

            dbEmailTemplate = new DbEmailTemplate
            {
                Id = emailTemplateId,
                AuthorId = Guid.NewGuid(),
                IsActive = true,
                Type = (int)EmailTemplateType.Greeting,
                CreatedAt = DateTime.UtcNow,
                EmailTemplateTexts = new List<DbEmailTemplateText>
                {
                    dbEmailTemplateText
                }
            };

            editDbEmailTemplate = new DbEmailTemplate
            {
                Id = dbEmailTemplate.Id,
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

        #region RemoveEmailTemplate
        [Test]
        public void ShouldThrowExceptionWhenEmailTemplateDoesNotExist()
        {
            Assert.Throws<NotFoundException>(() => repository.DisableEmailTemplate(Guid.NewGuid()));
            Assert.AreEqual(provider.EmailTemplates, new List<DbEmailTemplate> { dbEmailTemplate });
        }

        [Test]
        public void ShouldRemoveEmailTemplateSuccessfully()
        {
            repository.DisableEmailTemplate(emailTemplateId);

            Assert.IsTrue(provider.EmailTemplates.Find(emailTemplateId).IsActive == false);
            Assert.AreEqual(provider.EmailTemplates, new List<DbEmailTemplate> { dbEmailTemplate });
        }

        [Test]
        public void ShouldThrowExceptionWhenEmailTemplateIdNull()
        {
            Assert.Throws<NotFoundException>(() => repository.DisableEmailTemplate(Guid.Empty));
            Assert.AreEqual(provider.EmailTemplates, new List<DbEmailTemplate> { dbEmailTemplate });
        }
        #endregion

        #region AddEmailTemplate
        [Test]
        public void ShouldAddEmailTemplateCorrectly()
        {
            Assert.AreEqual(dbEmailTemplateToAdd.Id, repository.AddEmailTemplate(dbEmailTemplateToAdd));
            Assert.AreEqual(dbEmailTemplateToAdd, provider.EmailTemplates.Find(dbEmailTemplateToAdd.Id));
        }
        #endregion

        #region GetEmail
        [Test]
        public void ShouldThrowExceptionWhenEmailTemplateIdNotFound()
        {
            var emailTemplaiId = Guid.NewGuid();

            Assert.Throws<NotFoundException>(() => repository.GetEmailTemplateById(emailTemplaiId));
        }

        [Test]
        public void ShouldGetEmailTemplateByIdSuccessful()
        {
            var newDbEmailTemplate = repository.GetEmailTemplateById(dbEmailTemplate.Id);

            provider.MakeEntityDetached(dbEmailTemplate);
            newDbEmailTemplate.EmailTemplateTexts.ElementAt(0).EmailTemplate = null;

            SerializerAssert.AreEqual(dbEmailTemplate, newDbEmailTemplate);
        }

        [Test]
        public void ShouldThrowExceptionWhenEmailTemplateTypeNotFound()
        {
            Assert.Throws<NotFoundException>(() => repository.GetEmailTemplateByType((int)EmailTemplateType.Warning));
        }

        [Test]
        public void ShouldGetEmailTemplateByTypeSuccessful()
        {
            var newDbEmailTemplate = repository.GetEmailTemplateByType((int)EmailTemplateType.Greeting);

            provider.MakeEntityDetached(dbEmailTemplate);
            newDbEmailTemplate.EmailTemplateTexts.ElementAt(0).EmailTemplate = null;

            SerializerAssert.AreEqual(dbEmailTemplate, newDbEmailTemplate);
        }
        #endregion

        #region EditEmailTemplate
        [Test]
        public void ShouldThrowExceptionWhenEditEmailTemplateIdNotFound()
        {
            var newdbEmailTemplate = new DbEmailTemplate
            {
                Id = Guid.NewGuid(),
                AuthorId = editDbEmailTemplate.AuthorId,
                IsActive = editDbEmailTemplate.IsActive,
                CreatedAt = editDbEmailTemplate.CreatedAt
            };

            Assert.Throws<NotFoundException>(() => repository.EditEmailTemplate(newdbEmailTemplate));
        }

        [Test]
        public void ShouldEditEmailTemplateSuccessfully()
        {
            provider.MakeEntityDetached(dbEmailTemplate);

            repository.EditEmailTemplate(editDbEmailTemplate);

            SerializerAssert.AreEqual(editDbEmailTemplate, repository.GetEmailTemplateById(editDbEmailTemplate.Id));
        }
        #endregion
    }
}
