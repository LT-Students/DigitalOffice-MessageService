using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Data.Provider;
using LT.DigitalOffice.MessageService.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.MessageService.Data
{
  public class WorkspaceUserRepository : IWorkspaceUserRepository
  {
    private readonly IDataProvider _provider;

    public WorkspaceUserRepository(IDataProvider provider)
    {
      _provider = provider;
    }

    public async Task CreateAsync(IEnumerable<DbWorkspaceUser> dbWorkspaceUsers)
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

    public async Task<bool> WorkspaceUsersExist(List<Guid> usersIds, Guid workspaceId)
    {
      return (await _provider.WorkspacesUsers
        .Where(wu =>
          wu.WorkspaceId == workspaceId && usersIds.Contains(wu.UserId) && wu.IsActive)
        .ToListAsync())
        .Count == usersIds.Count;
    }
  }
}
