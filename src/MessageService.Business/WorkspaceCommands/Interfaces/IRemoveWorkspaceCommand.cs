using LT.DigitalOffice.MessageService.Models.Dto.Responses;
using System;

namespace LT.DigitalOffice.MessageService.Business.WorkspaceCommands.Interfaces
{
    public interface IRemoveWorkspaceCommand
    {
        OperationResultResponse<bool> Execute(Guid workspaceId);
    }
}
