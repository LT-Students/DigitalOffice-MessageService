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
    Task CreateAsync(IEnumerable<DbWorkspaceUser> dbWorkspaceUsers);

    Task<List<DbWorkspaceUser>> GetAdminsAsync(Guid workspaceId);

    Task<DbWorkspaceUser> GetAsync(Guid workspaseId, Guid userId);

    Task<bool> WorkspaceUsersExist(List<Guid> usersIds, Guid workspaceId);
  }
}
