using System;
using System.Collections.Generic;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace;

namespace LT.DigitalOffice.MessageService.Mappers.Db.Interfaces
{
  [AutoInject]
  public interface IDbWorkspaceMapper
  {
    DbWorkspace Map(CreateWorkspaceRequest request, List<Guid> usersIds);

    DbWorkspace Map(string name, List<Guid> usersIds, Guid createdBy);
  }
}
