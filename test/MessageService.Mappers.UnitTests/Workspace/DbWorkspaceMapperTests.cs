using LT.DigitalOffice.MessageService.Mappers.Db;
using LT.DigitalOffice.MessageService.Mappers.Db.Workspace;
using LT.DigitalOffice.MessageService.Mappers.Db.Workspace.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Models;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace;
using LT.DigitalOffice.Models.Broker.Requests.Message;
using LT.DigitalOffice.UnitTestKernel;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.MessageService.Mappers.UnitTests.Workspace
{
    public class DbWorkspaceMapperTests
    {
        private IDbWorkspaceMapper _mapper;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _mapper = new DbWorkspaceMapper();
        }

        #region WorkspaceRequest Map tests

        [Test]
        public void ShouldThrowArgumentNullExceptionWhenWorkspaceRequestIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _mapper.Map(null, Guid.NewGuid(), Guid.NewGuid()));
        }

        [Test]
        public void ShouldMapSuccessfulyWorkspaceRequest()
        {
            var imageId = Guid.NewGuid();
            var ownerId = Guid.NewGuid();

            var existingImage = new ImageInfo
            {
                Name = "name",
                Content = "context",
                Extension = "jpg"
            };

            var workspace = new WorkspaceRequest
            {
                Name = "Name",
                Description = "Description",
                Image = existingImage
            };

            var dbWorkspace = new DbWorkspace
            {
                Name = workspace.Name,
                OwnerId = ownerId,
                Description = workspace.Description,
                ImageId = imageId,
                IsActive = true
            };


            var result = _mapper.Map(workspace, ownerId, imageId);
            dbWorkspace.Id = result.Id;
            dbWorkspace.CreatedAt = result.CreatedAt;

            SerializerAssert.AreEqual(dbWorkspace, result);
        }

        #endregion

        #region ICreateWorkspaceRequest Map tests

        [Test]
        public void ShouldThrowArgumentNullExceptionWhenICreateWorkspaceRequestIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _mapper.Map(null));
        }

        [Test]
        public void ShouldMapSuccessfulyICreateWorkspaceRequest()
        {
            Guid creatorId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            string name = "Name";

            var request = new Mock<ICreateWorkspaceRequest>();
            request
                .Setup(x => x.Name)
                .Returns(name);
            request
                .Setup(x => x.Users)
                .Returns(new List<Guid>() { creatorId, userId });
            request
                .Setup(x => x.CreaterId)
                .Returns(creatorId);

            var dbWorkspace = new DbWorkspace
            {
                Name = name,
                OwnerId = creatorId,
                Description = "",
                IsActive = true
            };


            var result = _mapper.Map(request.Object);
            dbWorkspace.Id = result.Id;
            dbWorkspace.CreatedAt = result.CreatedAt;

            SerializerAssert.AreEqual(dbWorkspace, result);
        }

        #endregion
    }
}
