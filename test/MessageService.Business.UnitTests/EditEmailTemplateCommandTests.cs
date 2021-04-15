using FluentValidation;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Exceptions;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.MessageService.Business.EmailTemplatesCommands;
using LT.DigitalOffice.MessageService.Business.EmailTemplatesCommands.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Enums;
using LT.DigitalOffice.MessageService.Models.Dto.Models;
using LT.DigitalOffice.MessageService.Models.Dto.Requests;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.MessageService.Business.UnitTests
{
    class EditEmailTemplateCommandTests
    {
        private IEditEmailTemplateCommand command;
        private Mock<IAccessValidator> accessValidatorMock;
        private Mock<IEmailTemplateRepository> repositoryMock;
        private Mock<IValidator<EditEmailTemplateRequest>> validatorMock;
        private Mock<IMapper<EditEmailTemplateRequest, DbEmailTemplate>> mapperEmailTemplateMock;
        private Mock<IMapper<EmailTemplateTextInfo, DbEmailTemplateText>> mapperEmailTemplateTextMock;

        private Guid emailTemplateId;
        private DbEmailTemplate dbEmailTemplate;
        private DbEmailTemplateText editDbEmailTemplateText;
        private EmailTemplateTextInfo editEmailTemplateTextInfo;
        private DbEmailTemplate newDbEmailTemplate;
        private EditEmailTemplateRequest newEmailTemplate;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            emailTemplateId = Guid.NewGuid();

            editEmailTemplateTextInfo = new EmailTemplateTextInfo
            {
                Subject = "New subject",
                Text = "New email text",
                Language = "ru"
            };

            newEmailTemplate = new EditEmailTemplateRequest
            {
                Id = emailTemplateId,
                Name = "New pattern name",
                Type = EmailTemplateType.Greeting,
                EmailTemplateTexts = new List<EmailTemplateTextInfo>
                {
                    editEmailTemplateTextInfo
                }
            };

            dbEmailTemplate = new DbEmailTemplate
            {
                Id = emailTemplateId,
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
                        EmailTemplateId = emailTemplateId,
                        Subject = editEmailTemplateTextInfo.Subject,
                        Text = editEmailTemplateTextInfo.Text,
                        Language = "en"
                    },
                    new DbEmailTemplateText
                    {
                        Id = Guid.NewGuid(),
                        EmailTemplateId = emailTemplateId,
                        Subject = editEmailTemplateTextInfo.Subject,
                        Text = editEmailTemplateTextInfo.Text,
                        Language = editEmailTemplateTextInfo.Language
                    }
                }
            };

            newDbEmailTemplate = new()
            {
                Id = emailTemplateId,
                CreatedAt = dbEmailTemplate.CreatedAt,
                Name = newEmailTemplate.Name,
                Type = (int)newEmailTemplate.Type,
                AuthorId = dbEmailTemplate.AuthorId,
                IsActive = dbEmailTemplate.IsActive
            };

            editDbEmailTemplateText = new DbEmailTemplateText
            {
                Id = Guid.NewGuid(),
                EmailTemplateId = emailTemplateId,
                Subject = editEmailTemplateTextInfo.Subject,
                Text = editEmailTemplateTextInfo.Text,
                Language = editEmailTemplateTextInfo.Language
            };


            foreach (var templateText in newEmailTemplate.EmailTemplateTexts)
            {
                var dbEmailTemplateText = new DbEmailTemplateText
                {
                    Subject = templateText.Subject,
                    Text = templateText.Text,
                    Language = templateText.Language
                };

                newDbEmailTemplate.EmailTemplateTexts.Add(dbEmailTemplateText);
            }
        }

        [SetUp]
        public void SetUp()
        {
            repositoryMock = new Mock<IEmailTemplateRepository>();
            mapperEmailTemplateMock = new Mock<IMapper<EditEmailTemplateRequest, DbEmailTemplate>>();
            mapperEmailTemplateTextMock = new Mock<IMapper<EmailTemplateTextInfo, DbEmailTemplateText>>();
            accessValidatorMock = new Mock<IAccessValidator>();
            validatorMock = new Mock<IValidator<EditEmailTemplateRequest>>();

            command = new EditEmailTemplateCommand(
                accessValidatorMock.Object,
                repositoryMock.Object,
                validatorMock.Object,
                mapperEmailTemplateTextMock.Object,
                mapperEmailTemplateMock.Object);
        }

        [Test]
        public void ShouldThrowExceptionWhenUserHasNotRight()
        {
            accessValidatorMock
                .Setup(x => x.HasRights(3))
                .Returns(false);

            Assert.Throws<ForbiddenException>(() => command.Execute(newEmailTemplate));
        }

        [Test]
        public void ShouldThrowExceptionWhenRequestIsNotValid()
        {
            accessValidatorMock
                .Setup(x => x.HasRights(3))
                .Returns(true);

            validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
                .Returns(false);

            Assert.Throws<ValidationException>(() => command.Execute(newEmailTemplate));
        }


        [Test]
        public void ShouldThrowNullReferenceExceptionWhenEmailTemplateIdIsNotExist()
        {
            accessValidatorMock
                .Setup(x => x.HasRights(3))
                .Returns(true);

            validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
                .Returns(true);

            repositoryMock
                .Setup(x => x.GetEmailTemplateById(newEmailTemplate.Id))
                .Throws(new NullReferenceException());

            Assert.Throws<NullReferenceException>(() => command.Execute(newEmailTemplate));
        }

        public void ShouldThrowArgumentNullExceptionWhenRequestIsNull()
        {
            accessValidatorMock
                .Setup(x => x.HasRights(3))
                .Returns(true);

            validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
                .Returns(true);

            repositoryMock
                .Setup(x => x.GetEmailTemplateById(newEmailTemplate.Id))
                .Returns(newDbEmailTemplate);

            mapperEmailTemplateMock
                .Setup(mapper => mapper.Map(newEmailTemplate))
                .Throws(new ArgumentNullException());

            Assert.Throws<ArgumentNullException>(() => command.Execute(newEmailTemplate));
        }

        public void ShouldThrowNullReferenceExceptionWhenEmailTemplateDoesNotUpdate()
        {
            var dbEmailTemplateText = new DbEmailTemplateText
            {
                Id = Guid.NewGuid(),
                EmailTemplateId = emailTemplateId,
                Subject = editEmailTemplateTextInfo.Subject,
                Text = editEmailTemplateTextInfo.Text,
                Language = editEmailTemplateTextInfo.Language
            };

            accessValidatorMock
                .Setup(x => x.HasRights(3))
                .Returns(true);

            validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
                .Returns(true);

            repositoryMock
                .Setup(x => x.GetEmailTemplateById(newEmailTemplate.Id))
                .Returns(dbEmailTemplate);

            mapperEmailTemplateMock
                .Setup(mapper => mapper.Map(newEmailTemplate))
                .Returns(newDbEmailTemplate);

            mapperEmailTemplateTextMock
                .Setup(x => x.Map(editEmailTemplateTextInfo))
                .Returns(editDbEmailTemplateText);

            repositoryMock
                .Setup(x => x.EditEmailTemplate(newDbEmailTemplate))
                .Throws(new NullReferenceException());

            Assert.Throws<NullReferenceException>(() => command.Execute(newEmailTemplate));
        }

        [Test]
        public void ShouldEditEmailTemplateSuccessful()
        {
            accessValidatorMock
                .Setup(x => x.HasRights(3))
                .Returns(true);

            validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
                .Returns(true);

            repositoryMock
                .Setup(x => x.GetEmailTemplateById(newEmailTemplate.Id))
                .Returns(dbEmailTemplate);

            mapperEmailTemplateMock
                .Setup(mapper => mapper.Map(newEmailTemplate))
                .Returns(newDbEmailTemplate);

            mapperEmailTemplateTextMock
                .Setup(x => x.Map(editEmailTemplateTextInfo))
                .Returns(editDbEmailTemplateText);

            command.Execute(newEmailTemplate);

            mapperEmailTemplateMock.Verify();
            repositoryMock.Verify(repository => repository.GetEmailTemplateById(dbEmailTemplate.Id), Times.Once);
            repositoryMock.Verify(repository => repository.EditEmailTemplate(It.IsAny<DbEmailTemplate>()), Times.Once);
        }
    }
}
