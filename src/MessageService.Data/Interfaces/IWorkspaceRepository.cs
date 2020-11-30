using LT.DigitalOffice.MessageService.Models.Db;
using System;

namespace LT.DigitalOffice.MessageService.Data.Interfaces
{
    /// <summary>
    /// Represents interface of repository in repository pattern.
    /// Provides methods for working with Workspaces in the database of MessageService.
    /// </summary>
    public interface IWorkspaceRepository
    {
        /// <summary>
        /// Adds new workspace to the database.
        /// </summary>
        /// <param name="workspace">Workspace to add.</param>
        /// <returns>Guid of added workspace.</returns>
        Guid AddWorkspace(DbWorkspace workspace);
    }
}
