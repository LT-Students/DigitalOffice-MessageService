using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Db;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.MessageService.Data.Interfaces
{
    [AutoInject]
    public interface IWorkspaceUserRepository
    {
        void AddRange(IEnumerable<DbWorkspaceUser> workspaceUsers);
        List<DbWorkspaceUser> GetAdmins(Guid workspaceId);
    }
}
