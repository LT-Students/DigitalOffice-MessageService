using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Db;
using System;

namespace LT.DigitalOffice.MessageService.Mappers.Db.Interfaces
{
    [AutoInject]
    public interface IDbChannelMapper
    {
        DbChannel Map(Guid workspaceId, Guid ownerId, string name, bool isPrivate);
    }
}
