using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Data.Provider;
using LT.DigitalOffice.MessageService.Models.Db;
using Microsoft.AspNetCore.Http;
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

    public async Task<bool> CreateAsync(List<DbWorkspaceUser> dbWorkspaceUsers)
    {
      if (dbWorkspaceUsers == null || !dbWorkspaceUsers.Any())
      {
        return false;
      }

      _provider.WorkspacesUsers.AddRange(dbWorkspaceUsers);
      await _provider.SaveAsync();

      return true;
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

    public async Task<bool> WorkspaceUsersExist(List<Guid> workspaceUsersIds, Guid workspaceId)
    {
      return (await _provider.WorkspacesUsers
        .Where(wu =>
          wu.WorkspaceId == workspaceId && workspaceUsersIds.Contains(wu.Id) && wu.IsActive)
        .Select(wu => wu.Id)
        .ToListAsync())
        .Count == workspaceUsersIds.Count;
    }

    public async Task<bool> RemoveAsync(Guid workspaceId, IEnumerable<Guid> usersIds)
    {
      List<DbWorkspaceUser> dbWorkspaceUsers = await _provider.WorkspacesUsers
        .Where(wu => wu.IsActive && wu.WorkspaceId == workspaceId && usersIds.Contains(wu.UserId))
        .ToListAsync();

      if (!dbWorkspaceUsers.Any())
      {
        return false;
      }

      foreach (DbWorkspaceUser dbWorksapceUser in dbWorkspaceUsers)
      {
        dbWorksapceUser.IsActive = false;
        dbWorksapceUser.ModifiedAtUtc = DateTime.UtcNow;
        dbWorksapceUser.ModifiedBy = _httpContextAccessor.HttpContext.GetUserId();
      }

      _provider.WorkspacesUsers.UpdateRange(dbWorkspaceUsers);
      await _provider.SaveAsync();

      return true;
    }
  }
}
