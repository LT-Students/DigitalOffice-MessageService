using System;
using System.Collections.Generic;
using System.Linq;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Data.Provider;
using LT.DigitalOffice.MessageService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.UnitTestKernel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace LT.DigitalOffice.MessageService.Data.UnitTests
{
  internal class WorkspaceRepositoryTests
  {
    private IDataProvider _provider;
    private IWorkspaceRepository _repository;
    private Mock<IHttpContextAccessor> _accessorMock;

    private DbWorkspace _dbWorkspaceToAdd;
    private DbWorkspace _dbWorkspaceInDb;

    private Guid _userId = Guid.NewGuid();

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
      _dbWorkspaceToAdd = new DbWorkspace
      {
        Name = "Name",
        Description = "Description",
        IsActive = true,
      };

      _dbWorkspaceInDb = new DbWorkspace
      {
        Id = Guid.NewGuid(),
        Name = "Name",
        IsActive = true
      };

      CreateMemoryDb();
    }

    public void CreateMemoryDb()
    {
      var dbOptions = new DbContextOptionsBuilder<MessageServiceDbContext>()
             .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
             .Options;
      _provider = new MessageServiceDbContext(dbOptions);

      IDictionary<object, object> _items = new Dictionary<object, object>();
      _items.Add("UserId", _userId);

      _accessorMock = new();

      _accessorMock
          .Setup(x => x.HttpContext.Items)
          .Returns(_items);

      _repository = new WorkspaceRepository(_provider, _accessorMock.Object);
    }

    [SetUp]
    public void SetUp()
    {
      _provider.Workspaces.Add(_dbWorkspaceInDb);
      _provider.Save();
    }

    [TearDown]
    public void CleanDb()
    {
      if (_provider.IsInMemory())
      {
        _provider.EnsureDeleted();
      }
    }

    #region AddWorkspace

    [Test]
    public void ShouldAddWorkspaceCorrectly()
    {
      _dbWorkspaceToAdd.Id = Guid.NewGuid();

      _repository.Add(_dbWorkspaceToAdd);

      Assert.AreEqual(_dbWorkspaceToAdd, _provider.Workspaces.Find(_dbWorkspaceToAdd.Id));
    }

    #endregion

    #region GetWorkspace
    [Test]
    public void ShouldReturnExistsWorkspace()
    {
      SerializerAssert.AreEqual(_repository.Get(_dbWorkspaceInDb.Id), _dbWorkspaceInDb);
    }

    [Test]
    public void ShouldReturnNullWhenDoesNotExistWorkspaceWithThisId()
    {
      var incorrectId = Guid.NewGuid();

      Assert.IsNull(_repository.Get(incorrectId));
    }
    #endregion

    #region SwitchActiveStatus
    [Test]
    public void ShouldSwitchActiveStatusSuccessfully()
    {
      _dbWorkspaceInDb.IsActive = true;

      var id = _dbWorkspaceInDb.Id;

      Assert.IsTrue(_repository.SwitchActiveStatus(id, false));
      Assert.IsFalse(_provider.Workspaces.FirstOrDefault(w => w.Id == id).IsActive);
    }

    [Test]
    public void ShouldThrowNotFountExcWhenTryingSwitchStatusOfNonExistsWorkspace()
    {
      var id = Guid.NewGuid();

      Assert.IsFalse(_repository.SwitchActiveStatus(id, false));
    }
    #endregion
  }
}
