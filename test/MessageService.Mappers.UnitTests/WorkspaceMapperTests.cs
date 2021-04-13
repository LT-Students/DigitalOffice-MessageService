﻿using LT.DigitalOffice.MessageService.Mappers.WorkspaceMappers;
using LT.DigitalOffice.MessageService.Mappers.WorkspaceMappers.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests;
using LT.DigitalOffice.UnitTestKernel;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.MessageService.Mappers.UnitTests
{
    public class WorkspaceMapperTests
    {
        private IDbWorkspaceMapper _mapper;

        private Workspace _workspace;
        private DbWorkspace _dbWorkspace;
        private Guid _ownerId;
        private Guid? _imageId;
        private Image _existingImage;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _mapper = new DbWorkspaceMapper();

            _imageId = Guid.NewGuid();
            _ownerId = Guid.NewGuid();

            _existingImage = new Image
            {
                Name = "name",
                Content = "context",
                Extension = "jpg"
            };

            _workspace = new Workspace
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
