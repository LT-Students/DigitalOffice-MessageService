﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

    public async Task<Guid?> CreateAsync(DbWorkspace dbWorkspace)
    {
      if (dbWorkspace is null)
      {
        return null;
      }

      _provider.Workspaces.Add(dbWorkspace);
      await _provider.SaveAsync();

      return dbWorkspace.Id;
    }

    public async Task<bool> EditAsync(DbWorkspace dbWorkspace, JsonPatchDocument<DbWorkspace> request)
    {
      if (dbWorkspace is null || request is null)
      {
        return false;
      }

      request.ApplyTo(dbWorkspace);
      dbWorkspace.ModifiedBy = _httpContextAccessor.HttpContext.GetUserId();
      dbWorkspace.ModifiedAtUtc = DateTime.UtcNow;
      await _provider.SaveAsync();

      return true;
    }

    public async Task<(List<DbWorkspace> dbWorkspases, int totalCount)> FindAsync(FindWorkspaceFilter filter)
    {
      if (filter is null)
      {
        return (null, default);
      }

      IQueryable<DbWorkspace> dbWorkspaces = _provider.Workspaces.AsQueryable();

      if (!filter.IncludeDeactivated)
      {
        dbWorkspaces = dbWorkspaces.Where(w => w.IsActive);
      }

      Guid userId = _httpContextAccessor.HttpContext.GetUserId();

      dbWorkspaces = dbWorkspaces
        .Include(w => w.Users.Where(u => u.UserId == userId && u.IsActive))
        .Where(w => w.Users.Any());

      return (
        await dbWorkspaces.Skip(filter.SkipCount).Take(filter.TakeCount).ToListAsync(),
        await dbWorkspaces.CountAsync());
    }

    public async Task<DbWorkspace> GetAsync(
      Guid workspaceId,
      GetWorkspaceFilter filter)
    {
      IQueryable<DbWorkspace> dbWorkspace = _provider.Workspaces.AsQueryable();

      if (filter is not null && filter.IncludeChannels)
      {
        //to do add private channels filter
        dbWorkspace = dbWorkspace.Include(w => w.Channels.Where(c => c.IsActive));
      }

      if (filter is not null && filter.IncludeUsers)
      {
        dbWorkspace = dbWorkspace.Include(w => w.Users.Where(wu => wu.IsActive));
      }

      return await dbWorkspace.FirstOrDefaultAsync(w => w.Id == workspaceId);
    }
  }
}
