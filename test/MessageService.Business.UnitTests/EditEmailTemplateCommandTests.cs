using FluentValidation;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Exceptions;
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
        private Mock<IMapper<EditEmailTemplateRequest, DbEmailTemplate>> mapperMock;

        private Guid emailTemplateId;
        private DbEmailTemplate dbEmailTemplate;
        private DbEmailTemplate newDbEmailTemplate;
        private EditEmailTemplateRequest newEmailTemplate;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            emailTemplateId = Guid.NewGuid();

            newEmailTemplate = new EditEmailTemplateRequest
            {
                Id = emailTemplateId,
                Name = "New pattern name",
                Type = EmailTemplateType.Greeting,
                EmailTemplateTexts = new List<EmailTemplateTextInfo>
                {
                    new EmailTemplateTextInfo
                    {
                        Subject = "New subject",
                        Text = "New email text",
                        Language = "ru"
                    }
                }
            };

            dbEmailTemplate = new DbEmailTemplate
            {
                Id = emailTemplateId,
                CreatedAt = DateTime.UtcNow,
                Name = "Other pattern name",
                Type = 2,
                AuthorId = Guid.NewGuid(),
                IsActive = true
            };

            newDbEmailTemplate = new();
            newDbEmailTemplate.Id = emailTemplateId;
            newDbEmailTemplate.CreatedAt = dbEmailTemplate.CreatedAt;
            newDbEmailTemplate.Name = newEmailTemplate.Name;
            newDbEmailTemplate.Type = (int)newEmailTemplate.Type;
            newDbEmailTemplate.AuthorId = dbEmailTemplate.AuthorId;
            newDbEmailTemplate.IsActive = dbEmailTemplate.IsActive;


            foreach (var templateText in newEmailTemplate.EmailTemplateTexts)
            {
                var dbEmailTemplateText = new DbEmailTemplateText();

                dbEmailTemplateText.Subject = templateText.Subject;
                dbEmailTemplateText.Text = templateText.Text;
                dbEmailTemplateText.Language = dbEmailTemplateText.Language;

                newDbEmailTemplate.EmailTemplateTexts.Add(dbEmailTemplateText);
            }
        }

        [SetUp]
        public void SetUp()
        {
            repositoryMock = new Mock<IEmailTemplateRepository>();
            mapperMock = new Mock<IMapper<EditEmailTemplateRequest, DbEmailTemplate>>();
            accessValidatorMock = new Mock<IAccessValidator>();
            validatorMock = new Mock<IValidator<EditEmailTemplateRequest>>();

            command = new EditEmailTemplateCommand(
                accessValidatorMock.Object,
                repositoryMock.Object,
                validatorMock.Object,
                mapperMock.Object);
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

            mapperMock
                .Setup(mapper => mapper.Map(newEmailTemplate))
                .Throws(new ArgumentNullException());

            Assert.Throws<ArgumentNullException>(() => command.Execute(newEmailTemplate));
        }

        public void ShouldThrowNullReferenceExceptionWhenEmailTemplateDoesNotUpdate()
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

            mapperMock
                .Setup(mapper => mapper.Map(newEmailTemplate))
                .Returns(newDbEmailTemplate);

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

            mapperMock
                .Setup(mapper => mapper.Map(It.IsAny<EditEmailTemplateRequest>()))
                .Returns(newDbEmailTemplate);

            command.Execute(newEmailTemplate);

            mapperMock.Verify();
            repositoryMock.Verify(repository => repository.GetEmailTemplateById(dbEmailTemplate.Id), Times.Once);
            repositoryMock.Verify(repository => repository.EditEmailTemplate(It.IsAny<DbEmailTemplate>()), Times.Once);
        }
    }
}
