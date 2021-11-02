using System.Collections.Generic;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Responses.Workspace;
using LT.DigitalOffice.Models.Broker.Models;

namespace LT.DigitalOffice.MessageService.Mappers.Models.Interfaces
{
  [AutoInject]
  public interface IWorkspaceInfoMapper
  {
    WorkspaceInfo Map(DbWorkspace workspace, List<UserData> users);
  }
}
