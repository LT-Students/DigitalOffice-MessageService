using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace;

namespace LT.DigitalOffice.MessageService.Mappers.Db.Interfaces
{
  [AutoInject]
  public interface IDbWorkspaceMapper
  {
    Task<DbWorkspace> MapAsync(CreateWorkspaceRequest request);

    DbWorkspace Map(string name, List<Guid> usersIds, Guid createdBy);
  }
}
