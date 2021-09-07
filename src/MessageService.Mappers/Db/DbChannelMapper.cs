using System;
using LT.DigitalOffice.MessageService.Mappers.Db.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests;

namespace LT.DigitalOffice.MessageService.Mappers.Db
{
  public class DbChannelMapper : IDbChannelMapper
  {
    public DbChannel Map(CreateChannelRequest request, Guid CreatedBy)
    {
      if (request == null)
      {
        return null;
      }

      return new()
      {
        Id = Guid.NewGuid(),
        WorkspaceId = request.WorkspaceId,
        Name = request.Name,
        IsPrivate = request.IsPrivate,
        IsActive = true,
        ImageContent = request.Image.Content,
        ImageExtension = request.Image.Extension,
        CreatedBy = CreatedBy,
        CreatedAtUtc = DateTime.UtcNow
      };
    }

    public DbChannel Map(Guid workspaceId, Guid createdBy)
    {
      string defaultChannelName = "General";

      return new DbChannel()
      {
        Id = Guid.NewGuid(),
        WorkspaceId = workspaceId,
        Name = defaultChannelName,
        IsPrivate = false,
        IsActive = true,
        CreatedBy = createdBy,
        CreatedAtUtc = DateTime.UtcNow
      };
    }
  }
}
