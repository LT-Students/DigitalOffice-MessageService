using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Db;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.MessageService.Data.Interfaces
{
    [AutoInject]
    public interface IUserRepository
    {
        List<DbWorkspaceAdmin> GetAdmins(Guid workspaceId);
    }
}
