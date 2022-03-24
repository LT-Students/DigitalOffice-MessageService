using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Db;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.MessageService.Data.Interfaces
{
  [AutoInject]
  public interface IWorkspaceUserRepository
  {
    Task CreateAsync(List<DbWorkspaceUser> dbWorkspaceUsers);

    Task RemoveAsync(Guid workspaceId, List<DbWorkspaceUser> users);

    Task<bool> IsAdminAsync(Guid workspaceId, Guid userId);

    Task<List<DbWorkspaceUser>> GetByWorkspaceIdAsync(Guid workspaseId);

    Task<bool> WorkspaceUsersExistAsync(List<Guid> usersIds, Guid workspaceId);

    Task<bool> EditAsync(Guid workspaceId, Guid userId, JsonPatchDocument<DbWorkspaceUser> document);
  }
}
