using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Data.Provider;
using LT.DigitalOffice.MessageService.Models.Db;
using System;

namespace LT.DigitalOffice.MessageService.Data
{
    public class WorkspaceRepository : IWorkspaceRepository
    {
        private readonly IDataProvider _provider;

        public WorkspaceRepository(IDataProvider provider)
        {
            _provider = provider;
        }

        public Guid CreateWorkspace(DbWorkspace workspace)
        {
            _provider.Workspaces.Add(workspace);
            _provider.Save();

            return workspace.Id;
        }
    }
}