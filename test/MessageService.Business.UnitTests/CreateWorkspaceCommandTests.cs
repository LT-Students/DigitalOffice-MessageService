using FluentValidation;
using FluentValidation.Results;
using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.Broker.Responses;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Exceptions;
using LT.DigitalOffice.Kernel.Extensions;
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
using System.Threading.Tasks;

namespace LT.DigitalOffice.MessageService.Business.UnitTests
{
    public class OperationResult<T> : IOperationResult<T>
    {
        public bool IsSuccess { get; set; }

        public List<string> Errors { get; set; }

        public T Body { get; set; }
    }

    public class CreateWorkspaceCommandTests
    {
        private Mock<IWorkspaceRepository> _repositoryMock;
        private Mock<IValidator<Workspace>> _validatorMock;
        private Mock<IDbWorkspaceMapper> _mapperMock;
        private Mock<IRequestClient<ICreateImageRequest>> _requestBrokerMock;
        private Mock<ILogger<CreateCommand>> _loggerMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private Mock<OperationResult<ICreateImageResponse>> _operationResultMock;
        private ICreateCommand _command;

        private Guid _workspaceId;
        private Guid _userId;
        private Guid? _imageId;
        private Workspace _workspace;
        private DbWorkspace _dbWorkspace;
        private ValidationResult _validationResultError;
        private Mock<ValidationResult> _validationResultIsValidMock;

        private void BrokerSetUp()
        {
            var brokerResponseMock = new Mock<Response<IOperationResult<ICreateImageResponse>>>();
            _requestBrokerMock = new Mock<IRequestClient<ICreateImageRequest>>();

            _operationResultMock = new Mock<OperationResult<ICreateImageResponse>>();

            _requestBrokerMock
                .Setup(x => x.GetResponse<IOperationResult<ICreateImageResponse>>(
                    It.IsAny<object>(), default, default))
                .Returns(Task.FromResult(brokerResponseMock.Object));

            //_operationResultMock.Setup(x => x.Body).Returns((ICreateImageResponse)brokerResponseMock.Object);
            //_operationResultMock.Setup(x => x.IsSuccess).Returns(true);
            //_operationResultMock.Setup(x => x.Errors).Returns(new List<string>());


            //_operationResultMock
            //    .Setup(x => x.Body.Id)
            //    .Returns(_imageId);

            brokerResponseMock
                .Setup(x => x.Message)
                .Returns(_operationResultMock.Object);
        }

        private void ClientRequestUp()
        {
            _httpContextAccessorMock
                .Setup(x => x.HttpContext.GetUserId())
                .Returns(_userId);
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _workspaceId = Guid.NewGuid();
            _imageId = Guid.NewGuid();
            _userId = Guid.NewGuid();

            _workspace = new Workspace
            {
                Name = "Name",
                Description = "Description",
            };

            _dbWorkspace = new DbWorkspace
            {
                Id = _workspaceId,
                OwnerId = _userId,
                Name = _workspace.Name,
                Description = _workspace.Description,
                ImageId = _imageId,
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
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _loggerMock = new Mock<ILogger<CreateCommand>>();
            _requestBrokerMock = new Mock<IRequestClient<ICreateImageRequest>>();
            _command = new CreateCommand(
                _repositoryMock.Object,
                _validatorMock.Object,
                _mapperMock.Object,
                _requestBrokerMock.Object,
                _loggerMock.Object,
                _httpContextAccessorMock.Object);

            BrokerSetUp();
            ClientRequestUp();
        }

        [Test]
        public void ShouldCreateWorkspaceCorrectly()
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
