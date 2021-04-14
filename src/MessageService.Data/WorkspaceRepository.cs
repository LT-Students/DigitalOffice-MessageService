using LT.DigitalOffice.Kernel.Exceptions;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Data.Provider;
using LT.DigitalOffice.MessageService.Models.Db;
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
            _provider.Workspaces.Add(workspace);
            _provider.Save();

            return workspace.Id;
        }

        public DbWorkspace GetWorkspace(Guid workspaceId)
        {
            var result = _provider.Workspaces.FirstOrDefault(w => w.Id == workspaceId);
            if (result == null)
            {
                throw new NotFoundException($"Workspace with ID '{workspaceId}' was not found.");
            }

            return result;
        }

        public bool SwitchActiveStatus(Guid workspaceId, bool status)
        {
            DbWorkspace dbWorkspace = _provider.Workspaces.FirstOrDefault(w => w.Id == workspaceId);
            if (dbWorkspace == null)
            {
                throw new NotFoundException($"Workspace with ID '{workspaceId}' was not found.");
            }

            dbWorkspace.IsActive = status;

            _provider.Workspaces.Update(dbWorkspace);
            _provider.Save();

            return true;
        }
    }
}