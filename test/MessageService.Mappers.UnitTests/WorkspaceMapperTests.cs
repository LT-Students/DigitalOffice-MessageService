using LT.DigitalOffice.MessageService.Mappers.Db;
using LT.DigitalOffice.MessageService.Mappers.WorkspaceMappers.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Models;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace;
using LT.DigitalOffice.UnitTestKernel;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.MessageService.Mappers.UnitTests
{
    public class WorkspaceMapperTests
    {
        private IDbWorkspaceMapper _mapper;

        private WorkspaceRequest _workspace;
        private DbWorkspace _dbWorkspace;
        private Guid _ownerId;
        private Guid? _imageId;
        private ImageInfo _existingImage;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _mapper = new DbWorkspaceMapper();

            _imageId = Guid.NewGuid();
            _ownerId = Guid.NewGuid();

            _existingImage = new ImageInfo
            {
                Name = "name",
                Content = "context",
                Extension = "jpg"
            };

            _workspace = new WorkspaceRequest
            {
                Name = "Name",
                Description = "Description",
                Image = _existingImage
            };

            _dbWorkspace = new DbWorkspace
            {
                Name = _workspace.Name,
                OwnerId = _ownerId,
                Description = _workspace.Description,
                ImageId = _imageId,
                IsActive = true
            };
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWhenWorkspaceRequestIsNull()
        {
            _workspace = null;

            Assert.Throws<ArgumentNullException>(() => _mapper.Map(_workspace, _ownerId, _imageId));
        }

        [Test]
        public void ShouldReturnRightModelWhenWorkspaceRequestIsMapped()
        {
            var result = _mapper.Map(_workspace, _ownerId, _imageId);
            _dbWorkspace.Id = result.Id;

            SerializerAssert.AreEqual(_dbWorkspace, result);
        }
    }
}
