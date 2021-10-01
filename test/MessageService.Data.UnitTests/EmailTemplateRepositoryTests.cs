﻿using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Data.Provider;
using LT.DigitalOffice.MessageService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.Models.Broker.Enums;
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

        /*[OneTimeSetUp]
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
                CreatedBy = Guid.NewGuid(),
                IsActive = true,
                CreatedAtUtc = DateTime.UtcNow,
                EmailTemplateTexts = new List<DbEmailTemplateText>
                {
                    dbEmailTemplateText
                }
            };

            _dbEmailTemplate = new DbEmailTemplate
            {
                Id = _emailTemplateId,
                CreatedBy = Guid.NewGuid(),
                IsActive = true,
                Type = (int)EmailTemplateType.Greeting,
                CreatedAtUtc = DateTime.UtcNow,
                EmailTemplateTexts = new List<DbEmailTemplateText>
                {
                    dbEmailTemplateText
                }
            };

            _editDbEmailTemplate = new DbEmailTemplate
            {
                Id = _dbEmailTemplate.Id,
                CreatedBy = _dbEmailTemplate.CreatedBy,
                IsActive = _dbEmailTemplate.IsActive,
                CreatedAtUtc = _dbEmailTemplate.CreatedAtUtc
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

        #region Add
        [Test]
        public void ShouldAddEmailTemplateCorrectly()
        {
            Assert.AreEqual(_dbEmailTemplateToAdd.Id, _repository.Add(_dbEmailTemplateToAdd));
            Assert.AreEqual(_dbEmailTemplateToAdd, _provider.EmailTemplates.Find(_dbEmailTemplateToAdd.Id));
        }
        #endregion

        #region Get
        [Test]
        public void ShouldThrowExceptionWhenEmailTemplateIdNotFound()
        {
            var emailTemplaiId = Guid.NewGuid();

            Assert.Throws<NotFoundException>(() => _repository.Get(emailTemplaiId));
        }

        [Test]
        public void ShouldGetEmailTemplateByIdSuccessful()
        {
            var newDbEmailTemplate = _repository.Get(_dbEmailTemplate.Id);

            _provider.MakeEntityDetached(_dbEmailTemplate);
            newDbEmailTemplate.EmailTemplateTexts.ElementAt(0).EmailTemplate = null;

            SerializerAssert.AreEqual(_dbEmailTemplate, newDbEmailTemplate);
        }

        [Test]
        public void ShouldThrowExceptionWhenEmailTemplateTypeNotFound()
        {
            Assert.Throws<NotFoundException>(() => _repository.Get((int)EmailTemplateType.Warning));
        }

        [Test]
        public void ShouldGetEmailTemplateByTypeSuccessful()
        {
            var newDbEmailTemplate = _repository.Get((int)EmailTemplateType.Greeting);

            _provider.MakeEntityDetached(_dbEmailTemplate);
            newDbEmailTemplate.EmailTemplateTexts.ElementAt(0).EmailTemplate = null;

            SerializerAssert.AreEqual(_dbEmailTemplate, newDbEmailTemplate);
        }
        #endregion

        #region Edit
        [Test]
        public void ShouldThrowExceptionWhenEditEmailTemplateIdNotFound()
        {
            var newdbEmailTemplate = new DbEmailTemplate
            {
                Id = Guid.NewGuid(),
                CreatedBy = _editDbEmailTemplate.CreatedBy,
                IsActive = _editDbEmailTemplate.IsActive,
                CreatedAtUtc = _editDbEmailTemplate.CreatedAtUtc
            };

            Assert.Throws<NotFoundException>(() => _repository.Edit(newdbEmailTemplate));
        }

        //[Test]
        //public void ShouldEditEmailTemplateSuccessfully()
        //{
        //    _provider.MakeEntityDetached(_dbEmailTemplate);

        //    _repository.EditEmailTemplate(_editDbEmailTemplate);

        //    SerializerAssert.AreEqual(_editDbEmailTemplate, _repository.GetEmailTemplateById(_editDbEmailTemplate.Id));
        //}
        #endregion

        #region Find

        [Test]
        public void ShouldThrowBadRequestExceptionWhenSkipCountLessThanZero()
        {
            Assert.Throws<BadRequestException>(() => _repository.Find(-1, 5, true, out int totalCount));
        }

        [Test]
        public void ShouldThrowBadRequestExceptionWhenTakeCountLessOrEqualThanZero()
        {
            Assert.Throws<BadRequestException>(() => _repository.Find(0, -5, true, out int totalCount));
            Assert.Throws<BadRequestException>(() => _repository.Find(0, 0, true, out int totalCount));
        }

        [Test]
        public void ShouldReturnTemplatesSuccessfuly()
        {
            DbEmailTemplate response = _repository.Find(0, 1, true, out int total)[0];

            Assert.AreEqual(1, total);
            Assert.AreEqual(_dbEmailTemplate.Id, response.Id);
            Assert.AreEqual(_dbEmailTemplate.Name, response.Name);
            Assert.AreEqual(_dbEmailTemplate.IsActive, response.IsActive);
            Assert.AreEqual(_dbEmailTemplate.CreatedBy, response.CreatedBy);
            Assert.AreEqual(_dbEmailTemplate.CreatedAtUtc, response.CreatedAtUtc);
            Assert.AreEqual(_dbEmailTemplate.Type, response.Type);
        }

        #endregion*/
    }
}
