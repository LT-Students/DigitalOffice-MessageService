using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Results;
using LT.DigitalOffice.Kernel.AccessValidator.Interfaces;
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
        private Mock<IMapper<EditEmailTemplateRequest, DbEmailTemplate, DbEmailTemplate>> mapperMock;
        private Mock<IAccessValidator> accessValidatorMock;
        private Mock<ValidationResult> validationResultIsValidMock;
        private Mock<IValidator<EditEmailTemplateRequest>> validatorMock;

        private Guid requestingUserId;
        private Guid emailId;
        private ValidationResult validationResultError;
        private EditEmailTemplateRequest emailTemplate;
        private DbEmailTemplate dbEmailTemplate;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            requestingUserId = Guid.NewGuid();
            emailId = Guid.NewGuid();

            emailTemplate = new EditEmailTemplateRequest
            {
                Subject = "Subject",
                Body = "Body"
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
            mapperMock = new Mock<IMapper<EditEmailTemplateRequest, DbEmailTemplate, DbEmailTemplate>>();
            accessValidatorMock = new Mock<IAccessValidator>();
            validatorMock = new Mock<IValidator<EditEmailTemplateRequest>>();

            command = new EditEmailTemplateCommand(
                accessValidatorMock.Object,
                repositoryMock.Object,
                validatorMock.Object,
                mapperMock.Object);
        }

        [Test]
        public void ShouldThrowExceptionWhenUserHasNoRight()
        {
            accessValidatorMock
                .Setup(x => x.HasRights(3))
                .Returns(true);

            validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()))
                .Returns(validationResultIsValidMock.Object);

            repositoryMock
                .Setup(x => x.AddEmailTemplate(It.IsAny<DbEmailTemplate>()))
                .Returns(emailId);

            mapperMock
                .Setup(mapper => mapper.Map(It.IsAny<EditEmailTemplateRequest>(), It.IsAny<DbEmailTemplate>()))
                .Returns(dbEmailTemplate);

            command.Execute(emailTemplate, requestingUserId);

            mapperMock.Verify();
            repositoryMock.Verify();
        }

        [Test]
        public void ShouldEditEmailTemplateSuccessful()
        {
            accessValidatorMock
                .Setup(x => x.HasRights(3))
                .Returns(true);

            validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()))
                .Returns(validationResultIsValidMock.Object);

            repositoryMock
                .Setup(x => x.AddEmailTemplate(It.IsAny<DbEmailTemplate>()))
                .Returns(emailId);

            mapperMock
                .Setup(mapper => mapper.Map(It.IsAny<EditEmailTemplateRequest>(), It.IsAny<DbEmailTemplate>()))
                .Returns(dbEmailTemplate);

            command.Execute(emailTemplate, requestingUserId);

            mapperMock.Verify();
            repositoryMock.Verify();
        }
    }
}
