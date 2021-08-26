﻿using System;
using System.Collections.Generic;
using System.Linq;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Data.Provider;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace.Filters;
using Microsoft.AspNetCore.Http;
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

      workspaces = workspaces
        .Include(w => w.Users.Where(u => u.Id == _httpContextAccessor.HttpContext.GetUserId()))
        .Where(w => w.Users.Any());

      totalCount = workspaces.Count();

      return workspaces.Skip(filter.SkipCount).Take(filter.TakeCount).ToList();
    }

    public DbWorkspace Get(Guid workspaceId)
    {
      var result = _provider.Workspaces.FirstOrDefault(w => w.Id == workspaceId);
      if (result == null)
      {
        throw new NotFoundException($"Workspace with ID '{workspaceId}' was not found.");
      }

      return result;
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

      return workspace.FirstOrDefault(w => w.Id == filter.WorkspaceId)
        ?? throw new NotFoundException($"Workspace with id: '{filter.WorkspaceId}' doesn't exist.");
    }

    public bool SwitchActiveStatus(Guid workspaceId, bool status)
    {
      DbWorkspace dbWorkspace = _provider.Workspaces.FirstOrDefault(w => w.Id == workspaceId);
      if (dbWorkspace == null)
      {
        throw new NotFoundException($"Workspace with ID '{workspaceId}' was not found.");
      }

      dbWorkspace.IsActive = status;
      dbWorkspace.ModifiedAtUtc = DateTime.UtcNow;

      _provider.Workspaces.Update(dbWorkspace);
      _provider.Save();

      return true;
    }
  }
}
