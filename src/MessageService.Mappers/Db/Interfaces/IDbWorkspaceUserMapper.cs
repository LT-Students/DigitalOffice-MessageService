using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Db;
using System;

namespace LT.DigitalOffice.MessageService.Mappers.Db.Interfaces
{
    [AutoInject]
    public interface IDbWorkspaceUserMapper
    {
        DbWorkspaceUser Map(Guid workspaceId, Guid userId, bool IsAdmin);
    }
}
