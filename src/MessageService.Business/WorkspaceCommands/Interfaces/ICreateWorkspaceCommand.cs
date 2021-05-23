using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace;
using LT.DigitalOffice.MessageService.Models.Dto.Responses;
using System;

namespace LT.DigitalOffice.MessageService.Business.WorkspaceCommands.Interfaces
{
    /// <summary>
    /// Represents interface for a command in command pattern.
    /// Provides method for adding a workspace.
    /// </summary>
    [AutoInject]
    public interface ICreateWorkspaceCommand
    {
        /// <summary>
        ///  Adding a new workspace.
        /// </summary>
        /// <param name="workspace">Workspace data.</param>
        /// <returns>Guid of the added workspace.</returns>
        OperationResultResponse<Guid> Execute(WorkspaceRequest workspace);
    }
}
