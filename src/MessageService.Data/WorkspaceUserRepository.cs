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

    public async Task<bool> IsAdminAsync(Guid workspaceId, Guid userId)
    {
      return await _provider.WorkspacesUsers
        .AnyAsync(wa =>
          wa.WorkspaceId == workspaceId
          && wa.UserId == userId
          && wa.IsAdmin
          && wa.IsActive);
    }

    public async Task<List<DbWorkspaceUser>> GetByWorkspaceIdAsync(Guid workspaseId)
    {
      return await _provider.WorkspacesUsers
        .Where(x => x.IsActive && x.WorkspaceId == workspaseId)
        .ToListAsync();
    }

    public async Task RemoveAsync(Guid workspaceId, List<DbWorkspaceUser> users)
    {
      if (users is null)
      {
        return;
      }

      Guid userId = _httpContextAccessor.HttpContext.GetUserId();

      users.ForEach(u =>
        {
          u.IsActive = false;
          u.ModifiedAtUtc = DateTime.UtcNow;
          u.ModifiedBy = userId;
        });

      await _provider.SaveAsync();
    }

    public async Task<bool> EditAsync(Guid workspaceId, Guid userId, JsonPatchDocument<DbWorkspaceUser> document)
    {
      DbWorkspaceUser user =
        await _provider.WorkspacesUsers
          .Where(u => u.WorkspaceId == workspaceId && u.UserId == userId)
          .FirstOrDefaultAsync();

      if (user is null)
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
