using LT.DigitalOffice.MessageService.Models.Db;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.MessageService.Data.Interfaces
{
    public interface IUserRepository
    {
        List<DbWorkspaceAdmin> GetAdmins(Guid workspaceId);
    }
}
