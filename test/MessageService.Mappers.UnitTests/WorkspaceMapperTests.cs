using LT.DigitalOffice.Kernel.Exceptions;
using LT.DigitalOffice.UnitTestKernel;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto;
using NUnit.Framework;
using System;
using LT.DigitalOffice.MessageService.Mappers.WorkspaceMappers;
using LT.DigitalOffice.MessageService.Mappers.WorkspaceMappers.Interfaces;

namespace LT.DigitalOffice.MessageService.Mappers.UnitTests
{
    public class WorkspaceMapperTests
    {
        private IDbWorkspaceMapper _mapper;

        private Workspace _workspace;
        private DbWorkspace _dbWorkspace;
        private Guid _ownerId;
        private Guid? _imageId;

        //[OneTimeSetUp]
        //public void OneTimeSetUp()
        //{
        //    mapper = new WorkspaceMapper();
        private const string _existingImage = "img.jpg";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _mapper = new DbWorkspaceMapper();

        //    workspaceRequest = new Workspace
        //    {
        //        Name = "Name",
        //        Description = "Description",
        //        Image = "img.jpg"
        //    };
            _imageId = Guid.NewGuid();
            _ownerId = Guid.NewGuid();

            _workspace = new Workspace
            {
                Name = "Name",
                Description = "Description",
                Image = _existingImage
            };

        //    dbWorkspace = new DbWorkspace
        //    {
        //        Name = workspaceRequest.Name,
        //        Description = workspaceRequest.Description,
        //        Image = workspaceRequest.Image,
        //        IsActive = true
        //    };
        //}
            _dbWorkspace = new DbWorkspace
            {
                Name = _workspace.Name,
                OwnerId = _ownerId,
                Description = _workspace.Description,
                ImageId = _imageId,
                IsActive = true
            };
        }

        //[Test]
        //public void ShouldThrowArgumentNullExceptionWhenWorkspaceRequestIsNull()
        //{
        //    workspaceRequest = null;
        [Test]
        public void ShouldThrowArgumentNullExceptionWhenWorkspaceRequestIsNull()
        {
            _workspace = null;

        //    Assert.Throws<BadRequestException>(() => mapper.Map(workspaceRequest));
        //}
            Assert.Throws<BadRequestException>(() => _mapper.Map(_workspace, _ownerId, _imageId));
        }

        //[Test]
        //public void ShouldReturnRightModelWhenWorkspaceRequestIsMapped()
        //{
        //    var result = mapper.Map(workspaceRequest);
        //    dbWorkspace.Id = result.Id;
        [Test]
        public void ShouldReturnRightModelWhenWorkspaceRequestIsMapped()
        {
            var result = _mapper.Map(_workspace, _ownerId, _imageId);
            _dbWorkspace.Id = result.Id;

        //    Assert.IsInstanceOf<Guid>(result.Id);
        //    SerializerAssert.AreEqual(dbWorkspace, result);
        //}
            SerializerAssert.AreEqual(_dbWorkspace, result);
        }
    }
}
