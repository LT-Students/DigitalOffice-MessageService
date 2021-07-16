using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Db;
using System;

namespace LT.DigitalOffice.MessageService.Data.Interfaces
{
    /// <summary>
    /// Represents interface of repository in repository pattern.
    /// Provides methods for working with Workspaces in the database of MessageService.
    /// </summary>
    [AutoInject]
    public interface IWorkspaceRepository
    {
        void Add(DbWorkspace workspace);

        DbWorkspace Get(Guid workspaceId);

        bool SwitchActiveStatus(Guid workspaceId, bool status);
    }
}
