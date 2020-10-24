using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Data.Provider;
using LT.DigitalOffice.MessageService.Models.Db;
using System;

namespace LT.DigitalOffice.MessageService.Data
{
    public class WorkspaceRepository : IWorkspaceRepository
    {
        private readonly IDataProvider provider;

        public WorkspaceRepository(IDataProvider provider)
        {
            this.provider = provider;
        }

        public Guid AddWorkspace(DbWorkspace workspace)
        {
            provider.Workspaces.Add(workspace);
            provider.Save();

            return workspace.Id;
        }
    }
}
