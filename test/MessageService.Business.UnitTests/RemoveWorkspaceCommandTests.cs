using LT.DigitalOffice.Kernel.Exceptions;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.MessageService.Business.WorkspaceCommands;
using LT.DigitalOffice.MessageService.Business.WorkspaceCommands.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Enums;
using LT.DigitalOffice.MessageService.Models.Dto.Responses;
using LT.DigitalOffice.UnitTestKernel;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.MessageService.Business.UnitTests
{
    class RemoveWorkspaceCommandTests
    {
        private IRemoveWorkspaceCommand _command;

        private Mock<IWorkspaceRepository> _workspaceRepositoryMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;

        private Guid _userId;
        private Guid _ownerId;
        private Guid _adminId;
        private DbWorkspace _existWorkspace;
        private DbWorkspaceAdmin _dbWorkspaceAdmin;

        private void ClientRequestConfigure(Guid id)
        {
            IDictionary<object, object> httpContextItems = new Dictionary<object, object>();

            httpContextItems.Add("UserId", id);

            _httpContextAccessorMock
                .Setup(x => x.HttpContext.Items)
                .Returns(httpContextItems);
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _userId = Guid.NewGuid();
            _ownerId = Guid.NewGuid();
            _adminId = Guid.NewGuid();

            _existWorkspace = new DbWorkspace
            {
                Id = Guid.NewGuid(),
                Name = "name",
                IsActive = true,
                ImageId = null,
                OwnerId = _ownerId,
                Description = "description"
            };

            _dbWorkspaceAdmin = new DbWorkspaceAdmin
            {
                Id = Guid.NewGuid(),
                UserId = _adminId,
                WorkspaceId = _existWorkspace.Id
            };
        }

        [SetUp]
        public void SetUp()
        {
            _workspaceRepositoryMock = new Mock<IWorkspaceRepository>();
            _workspaceRepositoryMock
                .Setup(x => x.GetWorkspace(It.IsAny<Guid>()))
                .Returns(_existWorkspace);
            _workspaceRepositoryMock
                .Setup(x => x.SwitchActiveStatus(It.IsAny<Guid>(), It.IsAny<bool>()))
                .Returns(true);

            _userRepositoryMock = new Mock<IUserRepository>();
            _userRepositoryMock
                .Setup(x => x.GetAdmins(_existWorkspace.Id))
                .Returns(new List<DbWorkspaceAdmin>() { _dbWorkspaceAdmin });

            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            _command = new RemoveWorkspaceCommand(
                _workspaceRepositoryMock.Object,
                _userRepositoryMock.Object,
                _httpContextAccessorMock.Object);
        }

        [Test]
        public void ShouldRemoveWorkspaceByOwner()
        {
            var expectedResult = new OperationResultResponse<bool>
            {
                Status = OperationResultStatusType.FullSuccess,
                Body = true
            };

            ClientRequestConfigure(_ownerId);

            SerializerAssert.AreEqual(expectedResult, _command.Execute(_existWorkspace.Id));
        }

        [Test]
        public void ShouldRemoveWorkspaceByAdmin()
        {
            var expectedResult = new OperationResultResponse<bool>
            {
                Status = OperationResultStatusType.FullSuccess,
                Body = true
            };

            ClientRequestConfigure(_adminId);

            SerializerAssert.AreEqual(expectedResult, _command.Execute(_existWorkspace.Id));
        }

        [Test]
        public void ShouldThrowExcWhenUserHasNotRigth()
        {
            ClientRequestConfigure(_userId);

            Assert.Throws<ForbiddenException>(() => _command.Execute(_existWorkspace.Id));
        }
    }
}
