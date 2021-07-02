using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Data.Provider;
using LT.DigitalOffice.MessageService.Models.Db;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.MessageService.Data
{
    public class WorkspaceUserRepository : IWorkspaceUserRepository
    {
        private readonly IDataProvider _provider;

        public WorkspaceUserRepository(IDataProvider provider)
        {
            _provider = provider;
        }

        public void AddRange(IEnumerable<DbWorkspaceUser> workspaceUsers)
        {
            _provider.WorkspaceUsers.AddRange(workspaceUsers);
            _provider.Save();
        }

        public List<DbWorkspaceUser> GetAdmins(Guid workspaceId)
        {
            return _provider.WorkspaceUsers.Where(wa => wa.WorkspaceId == workspaceId && wa.IsAdmin && wa.IsActive).ToList();
        }
    }
}
