using LT.DigitalOffice.MessageService.Mappers.Db.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using System;

namespace LT.DigitalOffice.MessageService.Mappers.Db
{
    public class DbChannelMapper : IDbChannelMapper
    {
        public DbChannel Map(Guid workspaceId, Guid ownerId, string name, bool isPrivate)
        {
            return new DbChannel
            {
                Id = Guid.NewGuid(),
                WorkspaceId = workspaceId,
                OwnerId = ownerId,
                Name = name,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                IsPrivate = isPrivate
            };
        }
    }
}
