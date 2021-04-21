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
        private IDataProvider _provider;
        private IEmailTemplateRepository _repository;

        private DbEmailTemplate _dbEmailTemplate;
        private Guid _emailTemplateId;
        private DbEmailTemplate _dbEmailTemplateToAdd;
        private DbEmailTemplate _editDbEmailTemplate;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var dbOptions = new DbContextOptionsBuilder<MessageServiceDbContext>()
                   .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                   .Options;
            _provider = new MessageServiceDbContext(dbOptions);

            _repository = new EmailTemplateRepository(_provider);
        }

        [SetUp]
        public void SetUp()
        {
            _emailTemplateId = Guid.NewGuid();

            var dbEmailTemplateText = new DbEmailTemplateText
            {
                Id = Guid.NewGuid(),
                EmailTemplateId = _emailTemplateId,
                Subject = "Subject",
                Text = "Hello, {userFirstName}!!! Your password: {userPassword}, enter it at login",
                Language = "en"
            };

            _dbEmailTemplateToAdd = new DbEmailTemplate
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

            _dbEmailTemplate = new DbEmailTemplate
            {
                Id = _emailTemplateId,
                AuthorId = Guid.NewGuid(),
                IsActive = true,
                Type = (int)EmailTemplateType.Greeting,
                CreatedAt = DateTime.UtcNow,
                EmailTemplateTexts = new List<DbEmailTemplateText>
                {
                    dbEmailTemplateText
                }
            };

            _editDbEmailTemplate = new DbEmailTemplate
            {
                Id = _dbEmailTemplate.Id,
                AuthorId = _dbEmailTemplate.AuthorId,
                IsActive = _dbEmailTemplate.IsActive,
                CreatedAt = _dbEmailTemplate.CreatedAt
            };

            _provider.EmailTemplates.Add(_dbEmailTemplate);
            _provider.Save();
        }

        [TearDown]
        public void CleanDb()
        {
            if (_provider.IsInMemory())
            {
                _provider.EnsureDeleted();
            }
        }

        #region RemoveEmailTemplate
        [Test]
        public void ShouldThrowExceptionWhenEmailTemplateDoesNotExist()
        {
            Assert.Throws<NotFoundException>(() => _repository.DisableEmailTemplate(Guid.NewGuid()));
            Assert.AreEqual(_provider.EmailTemplates, new List<DbEmailTemplate> { _dbEmailTemplate });
        }

        [Test]
        public void ShouldRemoveEmailTemplateSuccessfully()
        {
            _repository.DisableEmailTemplate(_emailTemplateId);

            Assert.IsTrue(_provider.EmailTemplates.Find(_emailTemplateId).IsActive == false);
            Assert.AreEqual(_provider.EmailTemplates, new List<DbEmailTemplate> { _dbEmailTemplate });
        }

        [Test]
        public void ShouldThrowExceptionWhenEmailTemplateIdNull()
        {
            Assert.Throws<NotFoundException>(() => _repository.DisableEmailTemplate(Guid.Empty));
            Assert.AreEqual(_provider.EmailTemplates, new List<DbEmailTemplate> { _dbEmailTemplate });
        }
        #endregion

        #region AddEmailTemplate
        [Test]
        public void ShouldAddEmailTemplateCorrectly()
        {
            Assert.AreEqual(_dbEmailTemplateToAdd.Id, _repository.AddEmailTemplate(_dbEmailTemplateToAdd));
            Assert.AreEqual(_dbEmailTemplateToAdd, _provider.EmailTemplates.Find(_dbEmailTemplateToAdd.Id));
        }
        #endregion

        #region GetEmail
        [Test]
        public void ShouldThrowExceptionWhenEmailTemplateIdNotFound()
        {
            var emailTemplaiId = Guid.NewGuid();

            Assert.Throws<NotFoundException>(() => _repository.GetEmailTemplateById(emailTemplaiId));
        }

        [Test]
        public void ShouldGetEmailTemplateByIdSuccessful()
        {
            var newDbEmailTemplate = _repository.GetEmailTemplateById(_dbEmailTemplate.Id);

            _provider.MakeEntityDetached(_dbEmailTemplate);
            newDbEmailTemplate.EmailTemplateTexts.ElementAt(0).EmailTemplate = null;

            SerializerAssert.AreEqual(_dbEmailTemplate, newDbEmailTemplate);
        }

        [Test]
        public void ShouldThrowExceptionWhenEmailTemplateTypeNotFound()
        {
            Assert.Throws<NotFoundException>(() => _repository.GetEmailTemplateByType((int)EmailTemplateType.Warning));
        }

        [Test]
        public void ShouldGetEmailTemplateByTypeSuccessful()
        {
            var newDbEmailTemplate = _repository.GetEmailTemplateByType((int)EmailTemplateType.Greeting);

            _provider.MakeEntityDetached(_dbEmailTemplate);
            newDbEmailTemplate.EmailTemplateTexts.ElementAt(0).EmailTemplate = null;

            SerializerAssert.AreEqual(_dbEmailTemplate, newDbEmailTemplate);
        }
        #endregion

        #region EditEmailTemplate
        [Test]
        public void ShouldThrowExceptionWhenEditEmailTemplateIdNotFound()
        {
            var newdbEmailTemplate = new DbEmailTemplate
            {
                Id = Guid.NewGuid(),
                AuthorId = _editDbEmailTemplate.AuthorId,
                IsActive = _editDbEmailTemplate.IsActive,
                CreatedAt = _editDbEmailTemplate.CreatedAt
            };

            Assert.Throws<NotFoundException>(() => _repository.EditEmailTemplate(newdbEmailTemplate));
        }

        [Test]
        public void ShouldEditEmailTemplateSuccessfully()
        {
            _provider.MakeEntityDetached(_dbEmailTemplate);

            _repository.EditEmailTemplate(_editDbEmailTemplate);

            SerializerAssert.AreEqual(_editDbEmailTemplate, _repository.GetEmailTemplateById(_editDbEmailTemplate.Id));
        }
        #endregion
    }
}
