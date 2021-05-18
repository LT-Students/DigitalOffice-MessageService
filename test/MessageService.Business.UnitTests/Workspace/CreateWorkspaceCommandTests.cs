using FluentValidation;
using FluentValidation.Results;
using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.Broker.Responses;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.MessageService.Business.WorkspaceCommands;
using LT.DigitalOffice.MessageService.Business.WorkspaceCommands.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.WorkspaceMappers.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Models;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace;
using LT.DigitalOffice.MessageService.Models.Dto.Responses;
using LT.DigitalOffice.MessageService.Validation.Workspace.Interfaces;
using LT.DigitalOffice.UnitTestKernel;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LT.DigitalOffice.MessageService.Business.UnitTests.Workspace
{
    public class CreateWorkspaceCommandTests
    {
        private Mock<IWorkspaceRepository> _repositoryMock;
        private Mock<ICreateWorkspaceValidator> _validatorMock;
        private Mock<IDbWorkspaceMapper> _mapperMock;
        private Mock<ILogger<CreateWorkspaceCommand>> _loggerMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;

        private Mock<IRequestClient<IAddImageRequest>> _requestBrokerMock;
        private Mock<Request<IRequestClient<IAddImageRequest>>> _requestMock;

        private Mock<IAddImageResponse> _responseMock;
        private Mock<IOperationResult<IAddImageResponse>> _operationResultMock;
        private Mock<Response<IOperationResult<IAddImageResponse>>> _brokerResponseMock;

        private ICreateWorkspaceCommand _command;

        private Guid _workspaceId;
        private Guid _userId;
        private Guid _imageId;
        private WorkspaceRequest _workspace;
        private DbWorkspace _dbWorkspace;
        private ValidationResult _validationResultError;
        private Mock<ValidationResult> _validationResultIsValidMock;

        private void BrokerSetUp()
        {
            _responseMock = new Mock<IAddImageResponse>();
            _responseMock
                .Setup(x => x.Id)
                .Returns(_imageId);

            _operationResultMock = new Mock<IOperationResult<IAddImageResponse>>();
            _operationResultMock
                .Setup(x => x.IsSuccess)
                .Returns(true);
            _operationResultMock
                .Setup(x => x.Errors)
                .Returns(new List<string>());
            _operationResultMock
                .Setup(x => x.Body)
                .Returns(_responseMock.Object);

            _brokerResponseMock = new Mock<Response<IOperationResult<IAddImageResponse>>>();
            _brokerResponseMock
                .Setup(x => x.Message)
                .Returns(_operationResultMock.Object);

            _requestBrokerMock = new Mock<IRequestClient<IAddImageRequest>>();
            _requestBrokerMock
                .Setup(x => x.GetResponse<IOperationResult<IAddImageResponse>>(
                    It.IsAny<object>(), default, default))
                .Returns(Task.FromResult(_brokerResponseMock.Object));

            _requestMock = new Mock<Request<IRequestClient<IAddImageRequest>>>();
            _requestMock
                .Setup(x => x.Task)
                .Returns(Task.FromResult(_requestBrokerMock.Object));
        }

        private void ClientRequestUp()
        {
            IDictionary<object, object> httpContextItems = new Dictionary<object, object>();

            httpContextItems.Add("UserId", _userId);

            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _httpContextAccessorMock
                .Setup(x => x.HttpContext.Items)
                .Returns(httpContextItems);
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _workspaceId = Guid.NewGuid();
            _imageId = Guid.NewGuid();
            _userId = Guid.NewGuid();

            var image = new ImageInfo
            {
                Name = "name",
                Content = "context",
                Extension = "jpg"
            };

            _workspace = new WorkspaceRequest
            {
                Name = "Name",
                Image = image,
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
            _repositoryMock
                .Setup(x => x.CreateWorkspace(It.IsAny<DbWorkspace>()))
                .Returns(_workspaceId);

            _mapperMock = new Mock<IDbWorkspaceMapper>();
            _mapperMock
                .Setup(mapper => mapper.Map(It.IsAny<WorkspaceRequest>(), It.IsAny<Guid>(), It.IsAny<Guid?>()))
                .Returns(_dbWorkspace);

            _validatorMock = new Mock<ICreateWorkspaceValidator>();
            _validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()))
                .Returns(_validationResultIsValidMock.Object);

            _loggerMock = new Mock<ILogger<CreateWorkspaceCommand>>();

            BrokerSetUp();
            ClientRequestUp();

            _command = new CreateWorkspaceCommand(
                _mapperMock.Object,
                _validatorMock.Object,
                _repositoryMock.Object,
                _loggerMock.Object,
                _httpContextAccessorMock.Object,
                _requestBrokerMock.Object);
        }

        [Test]
        public void ShouldCreateWorkspaceCorrectly()
        {
            var expectedResponse = new OperationResultResponse<Guid>
            {
                Status = Models.Dto.Enums.OperationResultStatusType.FullSuccess,
                Body = _workspaceId
            };

            SerializerAssert.AreEqual(expectedResponse, _command.Execute(_workspace));
        }

        [Test]
        public void ShouldCreateWorkspaceCorrectlyWhenAddImageRequestUnsuccessful()
        {
            _operationResultMock
                .Setup(x => x.IsSuccess)
                .Returns(false);
            _operationResultMock
                .Setup(x => x.Errors)
                .Returns(new List<string>() { "some error" });

            var expectedResponse = new OperationResultResponse<Guid>
            {
                Status = Models.Dto.Enums.OperationResultStatusType.PartialSuccess,
                Body = _workspaceId,
                Errors = new List<string> { $"Can not add image to user with id {_userId}. Please try again later." }
            };

            SerializerAssert.AreEqual(expectedResponse, _command.Execute(_workspace));
        }

        [Test]
        public void ShouldCreateWorkspaceCorrectlyWhenAddImageRequestFailed()
        {
            _requestBrokerMock
                .Setup(x => x.GetResponse<IOperationResult<IAddImageResponse>>(
                    It.IsAny<object>(), default, default))
                .Throws(new Exception());

            var expectedResponse = new OperationResultResponse<Guid>
            {
                Status = Models.Dto.Enums.OperationResultStatusType.PartialSuccess,
                Body = _workspaceId,
                Errors = new List<string> { $"Can not add image to user with id {_userId}. Please try again later." }
            };

            SerializerAssert.AreEqual(expectedResponse, _command.Execute(_workspace));
        }

        [Test]
        public void ShouldThrowExceptionWhenMapperThrowsIt()
        {
            _validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()))
                .Returns(_validationResultIsValidMock.Object)
                .Verifiable();

            _mapperMock
                .Setup(mapper => mapper.Map(It.IsAny<WorkspaceRequest>(), It.IsAny<Guid>(), It.IsAny<Guid?>()))
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
