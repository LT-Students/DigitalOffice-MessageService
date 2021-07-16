using LT.DigitalOffice.MessageService.Mappers.Db.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using System;

namespace LT.DigitalOffice.MessageService.Mappers.Db
{
    public class DbChannelUserMapper : IDbChannelUserMapper
    {
        public DbChannelUser Map(Guid channelId, Guid workspaceUserId, bool isAdmin)
        {
            return new DbChannelUser
            {
                Id = Guid.NewGuid(),
                ChannelId = channelId,
                WorkspaceUserId = workspaceUserId,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                IsAdmin = isAdmin
            };
        }
    }
}
