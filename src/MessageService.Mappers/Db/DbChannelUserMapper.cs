using System;
using LT.DigitalOffice.MessageService.Mappers.Db.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Channel;

namespace LT.DigitalOffice.MessageService.Mappers.Db
{
  public class DbChannelUserMapper : IDbChannelUserMapper
  {
    public DbChannelUser Map(Guid channelId, ChannelUserRequest request,  Guid createdBy)
    {
      if (request == null)
      {
        return null;
      }

      return new DbChannelUser()
      {
        Id = Guid.NewGuid(),
        WorkspaceUserId = request.WorkspaceUserId,
        ChannelId = channelId,
        IsAdmin = request.IsAdmin,
        IsActive = true,
        CreatedBy = createdBy,
        CreatedAtUtc = DateTime.UtcNow
      };
    }

    public DbChannelUser Map(Guid channelId, Guid workspaceUserId, bool isAdmin, Guid createdBy)
    {
      return new DbChannelUser()
      {
        Id = Guid.NewGuid(),
        WorkspaceUserId = workspaceUserId,
        ChannelId = channelId,
        IsAdmin = isAdmin,
        IsActive = true,
        CreatedBy = createdBy,
        CreatedAtUtc = DateTime.UtcNow
      };
    }
  }
}
