using FluentValidation;
using FluentValidation.Results;
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
    public class AddWorkspaceCommandTests
    {
        private Mock<IWorkspaceRepository> repositoryMock;
        private Mock<IValidator<AddWorkspaceRequest>> validatorMock;
        private Mock<IMapper<AddWorkspaceRequest, DbWorkspace>> mapperMock;
        private IAddWorkspaceCommand command;

        private Guid workspaceId;
        private AddWorkspaceRequest workspace;
        private DbWorkspace dbWorkspace;
        private ValidationResult validationResultError;
        private Mock<ValidationResult> validationResultIsValidMock;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            workspaceId = Guid.NewGuid();
            workspace = new AddWorkspaceRequest
            {
                Name = "Name",
                Description = "Description",
            };

            dbWorkspace = new DbWorkspace
            {
                Id = workspaceId,
                Name = workspace.Name,
                Description = workspace.Description,
                IsActive = true
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
            repositoryMock = new Mock<IWorkspaceRepository>();
            mapperMock = new Mock<IMapper<AddWorkspaceRequest, DbWorkspace>>();
            validatorMock = new Mock<IValidator<AddWorkspaceRequest>>();

            command = new AddWorkspaceCommand(repositoryMock.Object, validatorMock.Object, mapperMock.Object);
        }

        [Test]
        public void ShouldAddEmailTemplateCorrectly()
        {
            validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()))
                .Returns(validationResultIsValidMock.Object);

            mapperMock
                .Setup(mapper => mapper.Map(It.IsAny<AddWorkspaceRequest>()))
                .Returns(dbWorkspace);

            repositoryMock
                .Setup(x => x.AddWorkspace(It.IsAny<DbWorkspace>()))
                .Returns(workspaceId);

            Assert.That(command.Execute(workspace), Is.EqualTo(workspaceId));
        }

        [Test]
        public void ShouldThrowExceptionWhenMapperThrowsIt()
        {
            validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()))
                .Returns(validationResultIsValidMock.Object)
                .Verifiable();

            mapperMock
                .Setup(mapper => mapper.Map(It.IsAny<AddWorkspaceRequest>()))
                .Throws<Exception>();

            Assert.Throws<Exception>(() => command.Execute(workspace));

            validatorMock.Verify();
            mapperMock.Verify();
            repositoryMock.Verify(repository => repository.AddWorkspace(It.IsAny<DbWorkspace>()), Times.Never());
        }

        [Test]
        public void ShouldThrowExceptionWhenValidatorThrowsIt()
        {
            validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()))
                .Returns(validationResultError)
                .Verifiable();

            Assert.Throws<ValidationException>(() => command.Execute(workspace));

            validatorMock.Verify();
            repositoryMock.Verify(repository => repository.AddWorkspace(It.IsAny<DbWorkspace>()), Times.Never());
        }
    }
}
