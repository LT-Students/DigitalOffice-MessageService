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

    Task<List<DbWorkspaceUser>> GetAdminsAsync(Guid workspaceId);

    Task<DbWorkspaceUser> GetAsync(Guid workspaseId, Guid userId);

    Task<bool> WorkspaceUsersExistAsync(List<Guid> usersIds, Guid workspaceId);

    Task RemoveAsync(List<Guid> usersIds, Guid workspaceId);

    Task<bool> UpdateAsync(DbWorkspaceUser user, JsonPatchDocument<DbWorkspaceUser> document);
  }
}
