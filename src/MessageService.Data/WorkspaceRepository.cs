using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Data.Provider;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.Kernel.Exceptions;
using System;
using System.Linq;

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
            if (_provider.Workspaces.Any(w => w.Id == workspace.Id))
            {
                return _provider.Workspaces.FirstOrDefault(w => w.Id == workspace.Id).Id;
            }

            _provider.Workspaces.Add(workspace);
            _provider.Save();

            return workspace.Id;
        }
    }
}