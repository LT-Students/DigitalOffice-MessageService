using LT.DigitalOffice.MessageService.Mappers.Db.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using System;

namespace LT.DigitalOffice.MessageService.Mappers.Db
{
    public class DbWorkspaceUserMapper : IDbWorkspaceUserMapper
    {
        public DbWorkspaceUser Map(Guid workspaceId, Guid userId, bool IsAdmin)
        {
            return new DbWorkspaceUser
            {
                Id = Guid.NewGuid(),
                WorkspaceId = workspaceId,
                UserId = userId,
                IsAdmin = IsAdmin,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
