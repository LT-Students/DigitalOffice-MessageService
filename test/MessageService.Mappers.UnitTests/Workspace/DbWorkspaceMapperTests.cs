using System;
using System.Collections.Generic;
using System.Linq;
using LT.DigitalOffice.MessageService.Mappers.Db.Workspace;
using LT.DigitalOffice.MessageService.Mappers.Db.Workspace.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace;
using LT.DigitalOffice.UnitTestKernel;
using NUnit.Framework;

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

      var existingImage = new CreateImageRequest
      {
        Name = "name",
        Content = "context",
        Extension = "jpg"
      };

      var workspace = new CreateWorkspaceRequest
      {
        Name = "Name",
        Description = "Description",
        Image = existingImage
      };

      var dbWorkspace = new DbWorkspace
      {
        Name = workspace.Name,
        CreatedBy = ownerId,
        Description = workspace.Description,
        ImageId = imageId,
        IsActive = true,
        Channels = new List<DbChannel>
        {
          new DbChannel
          {
            Name = "General",
            IsActive = true,
            IsPrivate = false
          }
        }
      };


      var result = _mapper.Map(workspace, ownerId, imageId);

      Assert.AreEqual(dbWorkspace.Name, result.Name);
      Assert.AreEqual(dbWorkspace.CreatedBy, result.CreatedBy);
      Assert.AreEqual(dbWorkspace.Description, result.Description);
      Assert.AreEqual(dbWorkspace.ImageId, result.ImageId);
      Assert.AreEqual(dbWorkspace.IsActive, result.IsActive);
      Assert.AreEqual(dbWorkspace.Channels.ToList()[0].Name, result.Channels.ToList()[0].Name);
      Assert.AreEqual(dbWorkspace.Channels.ToList()[0].IsActive, result.Channels.ToList()[0].IsActive);
      Assert.AreEqual(dbWorkspace.Channels.ToList()[0].IsPrivate, result.Channels.ToList()[0].IsPrivate);
    }

    #endregion

    #region ICreateWorkspaceRequest Map tests

    [Test]
    public void ShouldThrowArgumentNullExceptionWhenICreateWorkspaceRequestIsNull()
    {
      Assert.Throws<ArgumentNullException>(() => _mapper.Map(null));
    }

    //[Test]
    //public void ShouldMapSuccessfulyICreateWorkspaceRequest()
    //{
    //    Guid creatorId = Guid.NewGuid();
    //    Guid userId = Guid.NewGuid();
    //    string name = "Name";

    //    var request = new Mock<ICreateWorkspaceRequest>();
    //    request
    //        .Setup(x => x.Name)
    //        .Returns(name);
    //    request
    //        .Setup(x => x.Users)
    //        .Returns(new List<Guid>() { creatorId, userId });
    //    request
    //        .Setup(x => x.CreaterId)
    //        .Returns(creatorId);

    //    var dbWorkspace = new DbWorkspace
    //    {
    //        Name = name,
    //        CreatedBy = creatorId,
    //        Description = "",
    //        IsActive = true
    //    };


    //    var result = _mapper.Map(request.Object);
    //    dbWorkspace.Id = result.Id;
    //    dbWorkspace.CreatedAtUtc = result.CreatedAtUtc;

    //    SerializerAssert.AreEqual(dbWorkspace, result);
    //}

    #endregion
  }
}
