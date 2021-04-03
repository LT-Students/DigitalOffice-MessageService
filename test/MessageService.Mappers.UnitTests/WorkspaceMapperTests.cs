using LT.DigitalOffice.Kernel.Exceptions;
using LT.DigitalOffice.UnitTestKernel;
using LT.DigitalOffice.MessageService.Mappers.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto;
using NUnit.Framework;
using System;
using MassTransit;
using LT.DigitalOffice.Broker.Requests;
using Moq;
using Microsoft.Extensions.Logging;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Broker.Responses;

namespace LT.DigitalOffice.MessageService.Mappers.UnitTests
{
    public class WorkspaceMapperTests
    {
        private IMapper<Workspace, DbWorkspace> _mapper;
        private Mock<IRequestClient<ICreateImageRequest>> _moqRequestClient;
        private Mock<ILogger<DbWorkspaceMapper>> _moqLogger;

        private Workspace _workspace;
        private DbWorkspace _dbWorkspace;

        private const string _existingImage = "img.jpg";
        private const string _doesNotExistingImage = "smthwrong.jpg";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _moqLogger = new Mock<ILogger<DbWorkspaceMapper>>();

            _moqRequestClient = new Mock<IRequestClient<ICreateImageRequest>>();

            _mapper = new WorkspaceMapper(
                _moqRequestClient.Object,
                _moqLogger.Object);

            _workspace = new Workspace
            {
                Name = "Name",
                Description = "Description",
                Image = _existingImage
            };

            _dbWorkspace = new DbWorkspace
            {
                Name = _workspace.Name,
                Description = _workspace.Description,
                ImageId = Guid.NewGuid(),
                IsActive = true
            };

            //_moqRequestClient
            //    .Setup(x => x.GetResponse<IOperationResult<ICreateImageResponse>>(It.IsAny<object>()).Result)
            //    .Returns(new IOperationResult<ICreateImageResponse> { Body = ICreateImageResponse.CreateObj(_dbWorkspace.ImageId)})
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWhenWorkspaceRequestIsNull()
        {
            _workspace = null;

            Assert.Throws<BadRequestException>(() => _mapper.Map(_workspace));
        }

        [Test]
        public void ShouldReturnRightModelWhenWorkspaceRequestIsMapped()
        {
            var result = _mapper.Map(_workspace);
            _dbWorkspace.Id = result.Id;

            Assert.IsInstanceOf<Guid>(result.Id);
            SerializerAssert.AreEqual(_dbWorkspace, result);
        }
    }
}
