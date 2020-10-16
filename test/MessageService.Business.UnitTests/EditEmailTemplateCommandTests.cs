using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Results;
using LT.DigitalOffice.Kernel.AccessValidator.Interfaces;
using LT.DigitalOffice.Kernel.Exceptions;
using LT.DigitalOffice.MessageService.Business.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.MessageService.Business.UnitTests
{
    class EditEmailTemplateCommandTests
    {
        private Mock<IEmailTemplateRepository> repositoryMock;
        private IEditEmailTemplateCommand command;
        private Mock<IMapper<EditEmailTemplateRequest, DbEmailTemplate>> mapperMock;
        private Mock<IAccessValidator> accessValidatorMock;
        private Mock<ValidationResult> validationResultIsValidMock;
        private Mock<IValidator<EditEmailTemplateRequest>> validatorMock;

        private Guid emailId;
        private ValidationResult validationResultError;
        private EditEmailTemplateRequest emailTemplate;
        private DbEmailTemplate dbEmailTemplate;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            emailId = Guid.NewGuid();

            emailTemplate = new EditEmailTemplateRequest
            {
                Id = emailId,
                Subject = "Subject_1",
                Body = "Body_1"
            };

            dbEmailTemplate = new DbEmailTemplate
            {
                Id = emailId,
                Subject = "Subject",
                Body = "Body",
                CreatedAt = DateTime.Now,
                IsActive = true,
                AuthorId = Guid.NewGuid()
            };

            validationResultError = new ValidationResult(
                new List<ValidationFailure>
                {
                    new ValidationFailure("error", "something", null)
                });

            validationResultIsValidMock = new Mock<ValidationResult>();

            validationResultIsValidMock
                .Setup(x => x.IsValid)
                .Returns(true);
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

            Assert.Throws<ForbiddenException>(() => command.Execute(emailTemplate));
        }

        [Test]
        public void ShouldThrowExceptionWhenRequestIsNotValid()
        {
            accessValidatorMock
                .Setup(x => x.HasRights(3))
                .Returns(true);

            validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()))
                .Returns(validationResultError);

            Assert.Throws<ValidationException>(() => command.Execute(emailTemplate));
        }


        [Test]
        public void ShouldThrowNullReferenceExceptionWhenEmailTemplateIdIsNotExist()
        {
            accessValidatorMock
                .Setup(x => x.HasRights(3))
                .Returns(true);

            validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()))
                .Returns(validationResultIsValidMock.Object);

            repositoryMock
                .Setup(x => x.GetEmailTemplateById(dbEmailTemplate.Id))
                .Throws(new NullReferenceException());

            Assert.Throws<NullReferenceException>(() => command.Execute(emailTemplate));
        }

        public void ShouldThrowArgumentNullExceptionWhenRequestIsNull()
        {
            accessValidatorMock
                .Setup(x => x.HasRights(3))
                .Returns(true);

            validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()))
                .Returns(validationResultIsValidMock.Object);

            repositoryMock
                .Setup(x => x.GetEmailTemplateById(dbEmailTemplate.Id))
                .Returns(dbEmailTemplate);

            mapperMock
                .Setup(mapper => mapper.Map(It.IsAny<EditEmailTemplateRequest>()))
                .Throws(new ArgumentNullException());

            Assert.Throws<ArgumentNullException>(() => command.Execute(emailTemplate));
        }

        public void ShouldThrowNullReferenceExceptionWhenEmailTemplateDoesNotUpdate()
        {
            accessValidatorMock
                .Setup(x => x.HasRights(3))
                .Returns(true);

            validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()))
                .Returns(validationResultIsValidMock.Object);

            repositoryMock
                .Setup(x => x.GetEmailTemplateById(dbEmailTemplate.Id))
                .Returns(dbEmailTemplate);

            mapperMock
                .Setup(mapper => mapper.Map(It.IsAny<EditEmailTemplateRequest>()))
                .Returns(dbEmailTemplate);

            repositoryMock
                .Setup(x => x.EditEmailTemplate(It.IsAny<DbEmailTemplate>()))
                .Throws(new NullReferenceException());

            Assert.Throws<NullReferenceException>(() => command.Execute(emailTemplate));
        }

        [Test]
        public void ShouldEditEmailTemplateSuccessful()
        {
            var expectedEmailTemplate = new DbEmailTemplate
            {
                Id = emailId,
                Subject = "Subject",
                Body = "Body",
                CreatedAt = new DateTime().Date,
                AuthorId = Guid.NewGuid()
            };

            accessValidatorMock
                .Setup(x => x.HasRights(3))
                .Returns(true);

            validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()))
                .Returns(validationResultIsValidMock.Object);

            repositoryMock
                .Setup(x => x.GetEmailTemplateById(dbEmailTemplate.Id))
                .Returns(dbEmailTemplate);

            mapperMock
                .Setup(mapper => mapper.Map(It.IsAny<EditEmailTemplateRequest>()))
                .Returns(dbEmailTemplate);

            command.Execute(emailTemplate);

            mapperMock.Verify();
            repositoryMock.Verify(repository => repository.GetEmailTemplateById(dbEmailTemplate.Id), Times.Once);
            repositoryMock.Verify(repository => repository.EditEmailTemplate(It.IsAny<DbEmailTemplate>()), Times.Once);
        }
    }
}
