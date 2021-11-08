using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Db;

namespace LT.DigitalOffice.MessageService.Data.Interfaces
{
  [AutoInject]
  public interface IWorkspaceUserRepository
  {
    Task<bool> CreateAsync(List<DbWorkspaceUser> dbWorkspaceUsers);

    Task<List<DbWorkspaceUser>> GetAdminsAsync(Guid workspaceId);

    Task<DbWorkspaceUser> GetAsync(Guid workspaseId, Guid userId);

    Task<bool> WorkspaceUsersExist(List<Guid> workspaceUsersIds, Guid workspaceId);

    Task<bool> RemoveAsync(Guid workspaceId, IEnumerable<Guid> usersIds);

    Task<bool> IsWorkspaceAdminAsync(Guid workspaceId, Guid userId);
  }
}
