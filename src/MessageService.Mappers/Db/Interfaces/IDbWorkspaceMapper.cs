using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace;
using LT.DigitalOffice.Models.Broker.Requests.Message;

namespace LT.DigitalOffice.MessageService.Mappers.Db.Workspace.Interfaces
{
  [AutoInject]
  public interface IDbWorkspaceMapper
  {
    DbWorkspace Map(CreateWorkspaceRequest value);

    DbWorkspace Map(ICreateWorkspaceRequest request);
  }
}
