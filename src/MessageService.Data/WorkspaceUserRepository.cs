using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Data.Provider;
using LT.DigitalOffice.MessageService.Models.Db;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.MessageService.Data
{
  public class WorkspaceUserRepository : IWorkspaceUserRepository
  {
    private readonly IDataProvider _provider;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public WorkspaceUserRepository(
      IDataProvider provider,
      IHttpContextAccessor httpContextAccessor)
    {
      _provider = provider;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task CreateAsync(List<DbWorkspaceUser> dbWorkspaceUsers)
    {
      _provider.WorkspacesUsers.AddRange(dbWorkspaceUsers);
      await _provider.SaveAsync();
    }

    public async Task<List<DbWorkspaceUser>> GetAdminsAsync(Guid workspaceId)
    {
      return await _provider.WorkspacesUsers
        .Where(wa => wa.WorkspaceId == workspaceId && wa.IsAdmin && wa.IsActive)
        .ToListAsync();
    }

    public async Task<DbWorkspaceUser> GetAsync(Guid workspaseId, Guid userId)
    {
      return await _provider.WorkspacesUsers
        .FirstOrDefaultAsync(x => x.WorkspaceId == workspaseId && x.UserId == userId);
    }

    public async Task RemoveAsync(List<Guid> usersIds, Guid workspaceId)
    {
      if (usersIds is null)
      {
        return;
      }

      Guid userId = _httpContextAccessor.HttpContext.GetUserId();

      await _provider.WorkspacesUsers
        .Where(u => u.IsActive && u.WorkspaceId == workspaceId && usersIds.Contains(u.UserId))
        .ForEachAsync(u =>
        {
          u.IsActive = false;
          u.ModifiedAtUtc = DateTime.UtcNow;
          u.ModifiedBy = userId;
        });

      await _provider.SaveAsync();
    }

    public async Task<bool> UpdateAsync(DbWorkspaceUser user, JsonPatchDocument<DbWorkspaceUser> document)
    {
      if (user is null || document is null)
      {
        return false;
      }

      document.ApplyTo(user);
      user.ModifiedBy = _httpContextAccessor.HttpContext.GetUserId();
      user.ModifiedAtUtc = DateTime.UtcNow;

      await _provider.SaveAsync();

      return true;
    }

    public async Task<bool> WorkspaceUsersExistAsync(List<Guid> usersIds, Guid workspaceId)
    {
      return (await _provider.WorkspacesUsers
        .Where(wu =>
          wu.WorkspaceId == workspaceId && usersIds.Contains(wu.UserId) && wu.IsActive)
        .ToListAsync())
        .Count == usersIds.Count;
    }
  }
}
