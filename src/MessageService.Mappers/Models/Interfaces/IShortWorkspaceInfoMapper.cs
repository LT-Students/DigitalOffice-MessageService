using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Responses.Workspace;

namespace LT.DigitalOffice.MessageService.Mappers.Models.Interfaces
{
  [AutoInject]
  public interface IShortWorkspaceInfoMapper
  {
    ShortWorkspaceInfo Map(DbWorkspace dbWorkspace);
  }
}
