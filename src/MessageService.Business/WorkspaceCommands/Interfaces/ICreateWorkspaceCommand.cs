using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace;
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
        Guid Execute(WorkspaceRequest workspace);
    }
}
