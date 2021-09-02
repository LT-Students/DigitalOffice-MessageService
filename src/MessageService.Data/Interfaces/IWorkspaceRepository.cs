using System;
using System.Collections.Generic;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace.Filters;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.MessageService.Data.Interfaces
{
  /// <summary>
  /// Represents interface of repository in repository pattern.
  /// Provides methods for working with Workspaces in the database of MessageService.
  /// </summary>
  [AutoInject]
  public interface IWorkspaceRepository
  {
    void Add(DbWorkspace workspace);

    DbWorkspace Get(Guid workspaceId);

    DbWorkspace Get(GetWorkspaceFilter filter);

    List<DbWorkspace> Find(FindWorkspaceFilter filter, out int totalCount);

    bool Edit(DbWorkspace workspace, JsonPatchDocument<DbWorkspace> request);
  }
}
