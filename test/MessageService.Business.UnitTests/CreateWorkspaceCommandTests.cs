using FluentValidation;
using FluentValidation.Results;
using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.Kernel.Exceptions;
using LT.DigitalOffice.MessageService.Business.WorkspaceCommands;
using LT.DigitalOffice.MessageService.Business.WorkspaceCommands.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.WorkspaceMappers.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.MessageService.Business.UnitTests
{
    public class CreateWorkspaceCommandTests
    {
        private Mock<IWorkspaceRepository> _repositoryMock;
        private Mock<IValidator<Workspace>> _validatorMock;
        private Mock<IDbWorkspaceMapper> _mapperMock;
        private Mock<IRequestClient<ICreateImageRequest>> _mockRequestClient;
        private Mock<ILogger<CreateCommand>> _mockLogger;
        private Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private ICreateCommand _command;

        private Guid _workspaceId;
        private Workspace _workspace;
        private DbWorkspace _dbWorkspace;
        private ValidationResult _validationResultError;
        private Mock<ValidationResult> _validationResultIsValidMock;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _workspaceId = Guid.NewGuid();
            _workspace = new Workspace
            {
                Name = "Name",
                Description = "Description",
            };

            _dbWorkspace = new DbWorkspace
            {
                Id = _workspaceId,
                OwnerId = Guid.NewGuid(),
                Name = _workspace.Name,
                Description = _workspace.Description,
                ImageId = Guid.NewGuid(),
                IsActive = true
            };

            _validationResultError = new ValidationResult(
                new List<ValidationFailure>
                {
                    new ValidationFailure("error", "something", null)
                });

            _validationResultIsValidMock = new Mock<ValidationResult>();

            _validationResultIsValidMock
                .Setup(x => x.IsValid)
                .Returns(true);
        }

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IWorkspaceRepository>();
            _mapperMock = new Mock<IDbWorkspaceMapper>();
            _validatorMock = new Mock<IValidator<Workspace>>();
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            _mockLogger = new Mock<ILogger<CreateCommand>>();
            _mockRequestClient = new Mock<IRequestClient<ICreateImageRequest>>();
            _command = new CreateCommand(
                _repositoryMock.Object,
                _validatorMock.Object,
                _mapperMock.Object,
                _mockRequestClient.Object,
                _mockLogger.Object,
                _mockHttpContextAccessor.Object);
        }

        [Test]
        public void ShouldAddEmailTemplateCorrectly()
        {
            _validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()))
                .Returns(_validationResultIsValidMock.Object);

            _mapperMock
                .Setup(mapper => mapper.Map(It.IsAny<Workspace>(), It.IsAny<Guid>(), It.IsAny<Guid?>()))
                .Returns(_dbWorkspace);

            _repositoryMock
                .Setup(x => x.CreateWorkspace(It.IsAny<DbWorkspace>()))
                .Returns(_workspaceId);

            Assert.That(_command.Execute(_workspace), Is.EqualTo(_workspaceId));
        }

        [Test]
        public void ShouldThrowExceptionWhenMapperThrowsIt()
        {
            _validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()))
                .Returns(_validationResultIsValidMock.Object)
                .Verifiable();

            _mapperMock
                .Setup(mapper => mapper.Map(It.IsAny<Workspace>(), It.IsAny<Guid>(), It.IsAny<Guid?>()))
                .Throws<BadRequestException>();

            Assert.Throws<BadRequestException>(() => _command.Execute(_workspace));

            _validatorMock.Verify();
            _mapperMock.Verify();
            _repositoryMock.Verify(repository => repository.CreateWorkspace(It.IsAny<DbWorkspace>()), Times.Never());
        }

        [Test]
        public void ShouldThrowExceptionWhenValidatorThrowsIt()
        {
            _validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()))
                .Returns(_validationResultError)
                .Verifiable();

            Assert.Throws<ValidationException>(() => _command.Execute(_workspace));

            _validatorMock.Verify();
            _repositoryMock.Verify(repository => repository.CreateWorkspace(It.IsAny<DbWorkspace>()), Times.Never());
        }
    }
}
