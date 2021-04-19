using FluentValidation;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.MessageService.Business.EmailTemplatesCommands;
using LT.DigitalOffice.MessageService.Business.EmailTemplatesCommands.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Db.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Enums;
using LT.DigitalOffice.MessageService.Models.Dto.Models;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.EmailTemplate;
using LT.DigitalOffice.MessageService.Validation.EmailTemplate.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.MessageService.Business.UnitTests.Workspace
{
    class EditEmailTemplateCommandTests
    {
        private IEditEmailTemplateCommand _command;
        private Mock<IAccessValidator> _accessValidatorMock;
        private Mock<IEmailTemplateRepository> _repositoryMock;
        private Mock<IEditEmailTemplateValidator> _validatorMock;
        private Mock<IEditDbEmailTemplateMapper> _mapperEmailTemplateMock;
        private Mock<IDbEmailTemplateTextMapper> _mapperEmailTemplateTextMock;

        private Guid _emailTemplateId;
        private DbEmailTemplate _dbEmailTemplate;
        private DbEmailTemplateText _editDbEmailTemplateText;
        private EmailTemplateTextInfo _editEmailTemplateTextInfo;
        private DbEmailTemplate _newDbEmailTemplate;
        private EditEmailTemplateRequest _newEmailTemplate;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _emailTemplateId = Guid.NewGuid();

            _editEmailTemplateTextInfo = new EmailTemplateTextInfo
            {
                Subject = "New subject",
                Text = "New email text",
                Language = "ru"
            };

            _newEmailTemplate = new EditEmailTemplateRequest
            {
                Id = _emailTemplateId,
                Name = "New pattern name",
                Type = EmailTemplateType.Greeting,
                EmailTemplateTexts = new List<EmailTemplateTextInfo>
                {
                    _editEmailTemplateTextInfo
                }
            };

            _dbEmailTemplate = new DbEmailTemplate
            {
                Id = _emailTemplateId,
                CreatedAt = DateTime.UtcNow,
                Name = "Other pattern name",
                Type = 2,
                AuthorId = Guid.NewGuid(),
                IsActive = true,
                EmailTemplateTexts = new List<DbEmailTemplateText>
                {
                    new DbEmailTemplateText
                    {
                        Id = Guid.NewGuid(),
                        EmailTemplateId = _emailTemplateId,
                        Subject = _editEmailTemplateTextInfo.Subject,
                        Text = _editEmailTemplateTextInfo.Text,
                        Language = "en"
                    },
                    new DbEmailTemplateText
                    {
                        Id = Guid.NewGuid(),
                        EmailTemplateId = _emailTemplateId,
                        Subject = _editEmailTemplateTextInfo.Subject,
                        Text = _editEmailTemplateTextInfo.Text,
                        Language = _editEmailTemplateTextInfo.Language
                    }
                }
            };

            _newDbEmailTemplate = new()
            {
                Id = _emailTemplateId,
                CreatedAt = _dbEmailTemplate.CreatedAt,
                Name = _newEmailTemplate.Name,
                Type = (int)_newEmailTemplate.Type,
                AuthorId = _dbEmailTemplate.AuthorId,
                IsActive = _dbEmailTemplate.IsActive
            };

            _editDbEmailTemplateText = new DbEmailTemplateText
            {
                Id = Guid.NewGuid(),
                EmailTemplateId = _emailTemplateId,
                Subject = _editEmailTemplateTextInfo.Subject,
                Text = _editEmailTemplateTextInfo.Text,
                Language = _editEmailTemplateTextInfo.Language
            };


            foreach (var templateText in _newEmailTemplate.EmailTemplateTexts)
            {
                var dbEmailTemplateText = new DbEmailTemplateText
                {
                    Subject = templateText.Subject,
                    Text = templateText.Text,
                    Language = templateText.Language
                };

                _newDbEmailTemplate.EmailTemplateTexts.Add(dbEmailTemplateText);
            }
        }

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IEmailTemplateRepository>();
            _mapperEmailTemplateMock = new Mock<IEditDbEmailTemplateMapper>();
            _mapperEmailTemplateTextMock = new Mock<IDbEmailTemplateTextMapper>();
            _accessValidatorMock = new Mock<IAccessValidator>();
            _validatorMock = new Mock<IEditEmailTemplateValidator>();

            _command = new EditEmailTemplateCommand(
                _accessValidatorMock.Object,
                _repositoryMock.Object,
                _validatorMock.Object,
                _mapperEmailTemplateTextMock.Object,
                _mapperEmailTemplateMock.Object);
        }

        [Test]
        public void ShouldThrowExceptionWhenUserHasNotRight()
        {
            _accessValidatorMock
                .Setup(x => x.HasRights(Rights.AddEditRemoveEmailTemplates))
                .Returns(false);

            Assert.Throws<ForbiddenException>(() => _command.Execute(_newEmailTemplate));
        }

        [Test]
        public void ShouldThrowExceptionWhenRequestIsNotValid()
        {
            _accessValidatorMock
                .Setup(x => x.HasRights(Rights.AddEditRemoveEmailTemplates))
                .Returns(true);

            _validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
                .Returns(false);

            Assert.Throws<ValidationException>(() => _command.Execute(_newEmailTemplate));
        }


        [Test]
        public void ShouldThrowNullReferenceExceptionWhenEmailTemplateIdIsNotExist()
        {
            _accessValidatorMock
                .Setup(x => x.HasRights(3))
                .Returns(true);

            _validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
                .Returns(true);

            _repositoryMock
                .Setup(x => x.GetEmailTemplateById(_newEmailTemplate.Id))
                .Throws(new NullReferenceException());

            Assert.Throws<NullReferenceException>(() => _command.Execute(_newEmailTemplate));
        }

        public void ShouldThrowArgumentNullExceptionWhenRequestIsNull()
        {
            _accessValidatorMock
                .Setup(x => x.HasRights(3))
                .Returns(true);

            _validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
                .Returns(true);

            _repositoryMock
                .Setup(x => x.GetEmailTemplateById(_newEmailTemplate.Id))
                .Returns(_newDbEmailTemplate);

            _mapperEmailTemplateMock
                .Setup(mapper => mapper.Map(_newEmailTemplate))
                .Throws(new ArgumentNullException());

            Assert.Throws<ArgumentNullException>(() => _command.Execute(_newEmailTemplate));
        }

        public void ShouldThrowNullReferenceExceptionWhenEmailTemplateDoesNotUpdate()
        {
            var dbEmailTemplateText = new DbEmailTemplateText
            {
                Id = Guid.NewGuid(),
                EmailTemplateId = _emailTemplateId,
                Subject = _editEmailTemplateTextInfo.Subject,
                Text = _editEmailTemplateTextInfo.Text,
                Language = _editEmailTemplateTextInfo.Language
            };

            _accessValidatorMock
                .Setup(x => x.HasRights(3))
                .Returns(true);

            _validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
                .Returns(true);

            _repositoryMock
                .Setup(x => x.GetEmailTemplateById(_newEmailTemplate.Id))
                .Returns(_dbEmailTemplate);

            _mapperEmailTemplateMock
                .Setup(mapper => mapper.Map(_newEmailTemplate))
                .Returns(_newDbEmailTemplate);

            _mapperEmailTemplateTextMock
                .Setup(x => x.Map(_editEmailTemplateTextInfo))
                .Returns(_editDbEmailTemplateText);

            _repositoryMock
                .Setup(x => x.EditEmailTemplate(_newDbEmailTemplate))
                .Throws(new NullReferenceException());

            Assert.Throws<NullReferenceException>(() => _command.Execute(_newEmailTemplate));
        }

        [Test]
        public void ShouldEditEmailTemplateSuccessful()
        {
            _accessValidatorMock
                .Setup(x => x.HasRights(Rights.AddEditRemoveEmailTemplates))
                .Returns(true);

            _validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
                .Returns(true);

            _repositoryMock
                .Setup(x => x.GetEmailTemplateById(_newEmailTemplate.Id))
                .Returns(_dbEmailTemplate);

            _mapperEmailTemplateMock
                .Setup(mapper => mapper.Map(_newEmailTemplate))
                .Returns(_newDbEmailTemplate);

            _mapperEmailTemplateTextMock
                .Setup(x => x.Map(_editEmailTemplateTextInfo))
                .Returns(_editDbEmailTemplateText);

            _command.Execute(_newEmailTemplate);

            _mapperEmailTemplateMock.Verify();
            _repositoryMock.Verify(repository => repository.GetEmailTemplateById(_dbEmailTemplate.Id), Times.Once);
            _repositoryMock.Verify(repository => repository.EditEmailTemplate(It.IsAny<DbEmailTemplate>()), Times.Once);
        }
    }
}
