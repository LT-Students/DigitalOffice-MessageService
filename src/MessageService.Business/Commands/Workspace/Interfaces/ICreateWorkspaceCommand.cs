using System;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace;

namespace LT.DigitalOffice.MessageService.Business.Commands.Workspace.Interfaces
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
    OperationResultResponse<Guid?> Execute(CreateWorkspaceRequest workspace);
  }
}
