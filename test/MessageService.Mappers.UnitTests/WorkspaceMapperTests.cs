using LT.DigitalOffice.Kernel.Exceptions;
using LT.DigitalOffice.UnitTestKernel;
using LT.DigitalOffice.MessageService.Mappers.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.MessageService.Mappers.UnitTests
{
    public class WorkspaceMappertests
    {
        private IMapper<AddWorkspaceRequest, DbWorkspace> mapper;

        private AddWorkspaceRequest workspaceRequest;
        private DbWorkspace dbWorkspace;

        //[OneTimeSetUp]
        //public void OneTimeSetUp()
        //{
        //    mapper = new WorkspaceMapper();

        //    workspaceRequest = new Workspace
        //    {
        //        Name = "Name",
        //        Description = "Description",
        //        Image = "img.jpg"
        //    };

        //    dbWorkspace = new DbWorkspace
        //    {
        //        Name = workspaceRequest.Name,
        //        Description = workspaceRequest.Description,
        //        Image = workspaceRequest.Image,
        //        IsActive = true
        //    };
        //}

        //[Test]
        //public void ShouldThrowArgumentNullExceptionWhenWorkspaceRequestIsNull()
        //{
        //    workspaceRequest = null;

        //    Assert.Throws<BadRequestException>(() => mapper.Map(workspaceRequest));
        //}

        //[Test]
        //public void ShouldReturnRightModelWhenWorkspaceRequestIsMapped()
        //{
        //    var result = mapper.Map(workspaceRequest);
        //    dbWorkspace.Id = result.Id;

        //    Assert.IsInstanceOf<Guid>(result.Id);
        //    SerializerAssert.AreEqual(dbWorkspace, result);
        //}
    }
}
