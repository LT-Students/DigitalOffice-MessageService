using System;
using System.Collections.Generic;
using System.Linq;
using LT.DigitalOffice.MessageService.Mappers.Db.Workspace.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace;
using LT.DigitalOffice.Models.Broker.Requests.Message;

namespace LT.DigitalOffice.MessageService.Mappers.Db.Workspace
{
  public class DbWorkspaceMapper : IDbWorkspaceMapper
  {
    private const string DefaultChannelName = "General";
    private const string DefaultWorkspaceDescription = "";

    public DbWorkspace Map(CreateWorkspaceRequest value, Guid ownerId, Guid? imageId)
    {
      if (value == null)
      {
        throw new ArgumentNullException(nameof(value));
      }

      Guid workspaceId = Guid.NewGuid();

      DbChannel newChannel =
          new DbChannel
          {
            Id = Guid.NewGuid(),
            WorkspaceId = workspaceId,
            CreatedBy = ownerId,
            Name = DefaultChannelName,
            CreatedAtUtc = DateTime.UtcNow,
            IsActive = true,
            IsPrivate = false
          };

      return new DbWorkspace
      {
        Id = workspaceId,
        CreatedBy = ownerId,
        Name = value.Name,
        Description = value.Description,
        ImageId = imageId,
        CreatedAtUtc = DateTime.UtcNow,
        IsActive = true,
        Channels = new HashSet<DbChannel> { newChannel }
      };
    }

    public DbWorkspace Map(ICreateWorkspaceRequest request)
    {
      if (request == null)
      {
        throw new ArgumentNullException(nameof(request));
      }

      Guid workspaceId = Guid.NewGuid();

      List<DbWorkspaceUser> workspaceUsers =
          request.Users?.Select(userId => new DbWorkspaceUser
          {
            Id = Guid.NewGuid(),
            WorkspaceId = workspaceId,
            UserId = userId,
            IsAdmin = request.CreaterId == userId,
            CreatedBy = request.CreaterId,
            CreatedAtUtc = DateTime.UtcNow,
            IsActive = true
          }).ToList();

      DbChannel newChannel =
          new DbChannel
          {
            Id = Guid.NewGuid(),
            WorkspaceId = workspaceId,
            CreatedBy = request.CreaterId,
            Name = DefaultChannelName,
            CreatedAtUtc = DateTime.UtcNow,
            IsActive = true,
            IsPrivate = false
          };

      List<DbChannelUser> channelUsers =
          workspaceUsers?.Select(wu => new DbChannelUser
          {
            Id = Guid.NewGuid(),
            ChannelId = newChannel.Id,
            WorkspaceUserId = wu.Id,
            CreatedBy = request.CreaterId,
            CreatedAtUtc = DateTime.UtcNow,
            IsActive = true,
            IsAdmin = wu.IsAdmin
          }).ToList();

      newChannel.Users = channelUsers.ToHashSet();

      return new DbWorkspace
      {
        Id = workspaceId,
        CreatedBy = request.CreaterId,
        Name = request.Name,
        Description = DefaultWorkspaceDescription, // TODO Create description for default workspace
        ImageId = null,
        CreatedAtUtc = DateTime.UtcNow,
        IsActive = true,
        Channels = new HashSet<DbChannel> { newChannel },
        Users = workspaceUsers.ToHashSet()
      };
    }
  }
}
