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
using LT.DigitalOffice.MessageService.Models.Dto.Responses;
using LT.DigitalOffice.MessageService.Validation.EmailTemplate.Interfaces;
using LT.DigitalOffice.Models.Broker.Enums;
using LT.DigitalOffice.UnitTestKernel;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.MessageService.Business.UnitTests.EmailTemplate
{
    public class CreateEmailTemplateCommandTests
    {
        private ICreateEmailTemplateCommand _command;
        private Mock<IAccessValidator> _accessValidatorMock;
        private Mock<IEmailTemplateRepository> _repositoryMock;
        private Mock<ICreateEmailTemplateValidator> _validatorMock;
        private Mock<IDbEmailTemplateMapper> _mapperMock;

        private Guid emailId;
        private EmailTemplateRequest emailTemplate;
        private DbEmailTemplate dbEmailTemplate;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            emailId = Guid.NewGuid();
            emailTemplate = new EmailTemplateRequest
            {
                Name = "Pattern name",
                Type = EmailTemplateType.Greeting,
                AuthorId = Guid.NewGuid(),
                EmailTemplateTexts = new List<EmailTemplateTextInfo>
                {
                    new EmailTemplateTextInfo
                    {
                        Subject = "Subject",
                        Text = "Email text",
                        Language = "en"
                    }
                }
            };

            dbEmailTemplate = new DbEmailTemplate
            {
                Id = emailId,
                Name = emailTemplate.Name,
                CreatedAt = DateTime.UtcNow,
                AuthorId = emailTemplate.AuthorId,
                IsActive = true
            };

            foreach (var templateText in emailTemplate.EmailTemplateTexts)
            {
                var dbEmailTemplateText = new DbEmailTemplateText()
                {
                    Subject = templateText.Subject,
                    Text = templateText.Text,
                    Language = templateText.Language
                };

                dbEmailTemplate.EmailTemplateTexts.Add(dbEmailTemplateText);
            }
        }

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IEmailTemplateRepository>();
            _mapperMock = new Mock<IDbEmailTemplateMapper>();
            _accessValidatorMock = new Mock<IAccessValidator>();
            _validatorMock = new Mock<ICreateEmailTemplateValidator>();

            _command = new CreateEmailTemplateCommand(
                _mapperMock.Object,
                _accessValidatorMock.Object,
                _repositoryMock.Object,
                _validatorMock.Object);
        }

        [Test]
        public void ShouldAddEmailTemplateCorrectly()
        {
            _accessValidatorMock
                .Setup(x => x.HasRights(Rights.AddEditRemoveEmailTemplates))
                .Returns(true);

            _validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
                .Returns(true);

            _repositoryMock
                .Setup(x => x.AddEmailTemplate(It.IsAny<DbEmailTemplate>()))
                .Returns(emailId);

            _mapperMock
                .Setup(mapper => mapper.Map(It.IsAny<EmailTemplateRequest>()))
                .Returns(dbEmailTemplate);

            var expectedResponse = new OperationResultResponse<Guid>
            {
                Status = OperationResultStatusType.FullSuccess,
                Body = emailId
            };

            SerializerAssert.AreEqual(expectedResponse, _command.Execute(emailTemplate));

            _mapperMock.Verify();
            _repositoryMock.Verify();
        }

        [Test]
        public void ShouldThrowExceptionWhenMapperThrowsIt()
        {
            _accessValidatorMock
                .Setup(x => x.HasRights(Rights.AddEditRemoveEmailTemplates))
                .Returns(true);

            _validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
                .Returns(true);

            _mapperMock
                .Setup(mapper => mapper.Map(It.IsAny<EmailTemplateRequest>()))
                .Throws<Exception>();

            Assert.Throws<Exception>(() => _command.Execute(emailTemplate));

            _mapperMock.Verify();
            _repositoryMock.Verify(repository => repository.AddEmailTemplate(It.IsAny<DbEmailTemplate>()), Times.Never());
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

            Assert.Throws<ValidationException>(() => _command.Execute(emailTemplate));
        }

        [Test]
        public void ShouldThrowExceptionWhenUserHasNoRight()
        {
            _accessValidatorMock
                .Setup(x => x.HasRights(Rights.AddEditRemoveEmailTemplates))
                .Returns(false);

            Assert.Throws<ForbiddenException>(() => _command.Execute(emailTemplate));
        }
    }
}