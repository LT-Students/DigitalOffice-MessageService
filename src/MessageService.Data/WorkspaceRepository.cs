using System;
using System.Collections.Generic;
using System.Linq;
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

    public Guid? Add(DbWorkspace workspace)
    {
      if (workspace == null)
      {
        return null;
      }

      _provider.Workspaces.Add(workspace);
      _provider.Save();

      return workspace.Id;
    }

    public bool Edit(DbWorkspace dbWorkspace, JsonPatchDocument<DbWorkspace> request, Guid editorId)
    {
      if (dbWorkspace == null || request == null)
      {
        return false;
      }

      request.ApplyTo(dbWorkspace);
      dbWorkspace.ModifiedBy = editorId;
      dbWorkspace.ModifiedAtUtc = DateTime.UtcNow;
      _provider.Save();

      return true;
    }

    public List<DbWorkspace> Find(FindWorkspaceFilter filter, out int totalCount, List<string> errors)
    {
      if (filter.SkipCount < 0)
      {
        errors.Add("Skip count cannot be less than 0.");
        totalCount = 0;
        return null;
      }

      if (filter.TakeCount < 1)
      {
        errors.Add("Take count cannot be less than 1.");
        totalCount = 0;
        return null;
      }

      IQueryable<DbWorkspace> dbWorkspaces = _provider.Workspaces.AsQueryable();

      if (!filter.IncludeDeactivated)
      {
        dbWorkspaces = dbWorkspaces.Where(w => w.IsActive);
      }

      Guid userId = _httpContextAccessor.HttpContext.GetUserId();

      dbWorkspaces = dbWorkspaces
        .Include(w => w.Users.Where(u => u.Id == userId))
        .Where(w => w.Users.Any() || w.CreatedBy == userId);

      totalCount = dbWorkspaces.Count();

      return dbWorkspaces.Skip(filter.SkipCount).Take(filter.TakeCount).ToList();
    }

    public DbWorkspace Get(Guid workspaceId)
    {
      return _provider.Workspaces.FirstOrDefault(w => w.Id == workspaceId);
    }

    public DbWorkspace Get(GetWorkspaceFilter filter)
    {
      IQueryable<DbWorkspace> dbWorkspace = _provider.Workspaces.AsQueryable();

      if (filter.IsIncludeChannels)
      {
        dbWorkspace = dbWorkspace.Include(w => w.Channels);
      }

      if (filter.IsIncludeUsers)
      {
        dbWorkspace = dbWorkspace.Include(w => w.Users);
      }

      return dbWorkspace.FirstOrDefault(w => w.Id == filter.WorkspaceId);
    }
  }
}
