using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Dto.Responses;
using System;

namespace LT.DigitalOffice.MessageService.Business.WorkspaceCommands.Interfaces
{
    [AutoInject]
    public interface IRemoveWorkspaceCommand
    {
        OperationResultResponse<bool> Execute(Guid workspaceId);
    }
}
