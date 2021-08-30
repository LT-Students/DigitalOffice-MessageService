using System;
using System.Collections.Generic;
using System.Linq;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Data.Provider;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.MessageService.Data
{
  public class WorkspaceRepository : IWorkspaceRepository
  {
    private readonly IDataProvider _provider;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public WorkspaceRepository(
      IDataProvider provider,
      IHttpContextAccessor httpContextAccessor)
    {
      _provider = provider;
      _httpContextAccessor = httpContextAccessor;
    }

    public void Add(DbWorkspace workspace)
    {
      _provider.Workspaces.Add(workspace);
      _provider.Save();
    }

    public bool Edit(DbWorkspace workspace, JsonPatchDocument<DbWorkspace> request)
    {
      if (workspace == null)
      {
        throw new ArgumentNullException(nameof(workspace));
      }

      request.ApplyTo(workspace);
      _provider.Save();

      return true;
    }

    public List<DbWorkspace> Find(FindWorkspaceFilter filter, out int totalCount)
    {
      if (filter.SkipCount < 0)
      {
        throw new BadRequestException("Skip count cannot be less than 0.");
      }

      if (filter.TakeCount < 1)
      {
        throw new BadRequestException("Take count cannot be less than 1.");
      }

      IQueryable<DbWorkspace> workspaces = _provider.Workspaces.AsQueryable();

      if (!filter.IsIncludeDeactivated)
      {
        workspaces = workspaces.Where(w => w.IsActive);
      }

      Guid userId = _httpContextAccessor.HttpContext.GetUserId();

      workspaces = workspaces
        .Include(w => w.Users.Where(u => u.Id == userId))
        .Where(w => w.Users.Any() || w.CreatedBy == userId);

      totalCount = workspaces.Count();

      return workspaces.Skip(filter.SkipCount).Take(filter.TakeCount).ToList();
    }

    public DbWorkspace Get(Guid workspaceId)
    {
      return _provider.Workspaces.FirstOrDefault(w => w.Id == workspaceId);
    }

    public DbWorkspace Get(GetWorkspaceFilter filter)
    {
      IQueryable<DbWorkspace> workspace = _provider.Workspaces.AsQueryable();

      if (filter.IsIncludeChannels)
      {
        workspace = workspace.Include(w => w.Channels);
      }

      if (filter.IsIncludeUsers)
      {
        workspace = workspace.Include(w => w.Users);
      }

      return workspace.FirstOrDefault(w => w.Id == filter.WorkspaceId);
    }

    public bool SwitchActiveStatus(Guid workspaceId, bool status)
    {
      DbWorkspace dbWorkspace = _provider.Workspaces.FirstOrDefault(w => w.Id == workspaceId);
      if (dbWorkspace == null)
      {
        return false;
      }

      dbWorkspace.IsActive = status;
      dbWorkspace.ModifiedAtUtc = DateTime.UtcNow;

      _provider.Workspaces.Update(dbWorkspace);
      _provider.Save();

      return true;
    }
  }
}
