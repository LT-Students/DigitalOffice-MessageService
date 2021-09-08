using System;
using System.Collections.Generic;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Db;

namespace LT.DigitalOffice.MessageService.Data.Interfaces
{
  [AutoInject]
  public interface IWorkspaceUserRepository
  {
    void AddRange(IEnumerable<DbWorkspaceUser> workspaceUsers);

    List<DbWorkspaceUser> GetAdmins(Guid workspaceId);

    DbWorkspaceUser Get(Guid workspaseId, Guid userId);

    List<Guid> AreExistWorkspaceUsers(List<Guid> workspaceUsersIds, Guid workspaceId);
  }
}
