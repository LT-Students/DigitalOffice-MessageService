using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Data.Provider;
using LT.DigitalOffice.MessageService.Models.Db;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.MessageService.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly IDataProvider _provider;

        public UserRepository(
            IDataProvider provider)
        {
            _provider = provider;
        }

        public List<DbWorkspaceAdmin> GetAdmins(Guid workspaceId)
        {
            return _provider.WorkspaceAdmins.Where(wa => wa.WorkspaceId == workspaceId).ToList();
        }
    }
}
