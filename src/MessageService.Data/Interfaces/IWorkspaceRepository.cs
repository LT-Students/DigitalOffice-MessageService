using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace.Filters;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.MessageService.Data.Interfaces
{
  [AutoInject]
  public interface IWorkspaceRepository
  {
    Task<Guid?> CreateAsync(DbWorkspace dbWorkspace);

    Task<DbWorkspace> GetAsync(Guid workspaceId, GetWorkspaceFilter filter = null);

    Task<(List<DbWorkspace> dbWorkspases, int totalCount)> FindAsync(FindWorkspaceFilter filter);

    Task<bool> EditAsync(DbWorkspace workspace, JsonPatchDocument<DbWorkspace> request);
  }
}
