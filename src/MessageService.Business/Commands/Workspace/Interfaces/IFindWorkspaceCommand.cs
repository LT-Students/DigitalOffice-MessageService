using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Models.Dto.Models;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace.Filters;

namespace LT.DigitalOffice.MessageService.Business.Commands.Workspace.Interfaces
{
    [AutoInject]
    public interface IFindWorkspaceCommand
    {
        FindResultResponse<ShortWorkspaceInfo> Execute(FindWorkspaceFilter filter);
    }
}
