using System;
using LT.DigitalOffice.MessageService.Mappers.Db.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;

namespace LT.DigitalOffice.MessageService.Mappers.Db
{
  public class DbWorkspaceUserMapper : IDbWorkspaceUserMapper
  {
    public DbWorkspaceUser Map(Guid userId, Guid workspaceId, bool isAdmin, Guid createdBy)
    {
      return new DbWorkspaceUser()
      {
        Id = Guid.NewGuid(),
        UserId = userId,
        WorkspaceId = workspaceId,
        IsAdmin = isAdmin,
        IsActive = true,
        CreatedBy = createdBy,
        CreatedAtUtc = DateTime.UtcNow
      };
    }
  }
}
