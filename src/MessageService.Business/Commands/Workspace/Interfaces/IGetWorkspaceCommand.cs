using System;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace.Filters;
using LT.DigitalOffice.MessageService.Models.Dto.Responses.Workspace;

namespace LT.DigitalOffice.MessageService.Business.Commands.Workspace.Interfaces
{
  [AutoInject]
  public interface IGetWorkspaceCommand
  {
    Task<OperationResultResponse<WorkspaceInfo>> ExecuteAsync(Guid workspaceId, GetWorkspaceFilter filter);
  }
}
