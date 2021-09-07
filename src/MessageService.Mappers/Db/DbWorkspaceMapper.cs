using System;
using System.Collections.Generic;
using System.Linq;
using LT.DigitalOffice.MessageService.Mappers.Db.Workspace.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Helpers.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace;
using LT.DigitalOffice.Models.Broker.Requests.Message;

namespace LT.DigitalOffice.MessageService.Mappers.Db.Workspace
{
  public class DbWorkspaceMapper : IDbWorkspaceMapper
  {
    private const string DefaultChannelName = "General";
    private const string DefaultWorkspaceDescription = "";
    private readonly IResizeImageHelper _resizeHelper;

    public DbWorkspaceMapper(IResizeImageHelper resizeHelper)
    {
      _resizeHelper = resizeHelper;
    }

    public DbWorkspace Map(CreateWorkspaceRequest request, Guid createdBy)
    {
      if (request == null)
      {
        return null;
      }

      return new DbWorkspace
      {
        Id = Guid.NewGuid(),
        Name = request.Name,
        Description = request.Description,
        IsActive = true,
        ImageContent = request.Image != null ?
          _resizeHelper.Resize(request.Image.Content, request.Image.Extension) : null,
        ImageExtension = request.Image?.Extension,
        CreatedBy = createdBy,
        CreatedAtUtc = DateTime.UtcNow
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
          Name = DefaultChannelName,
          CreatedBy = request.CreaterId,
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
        Name = request.Name,
        Description = DefaultWorkspaceDescription, // TODO Create description for default workspace
        IsActive = true,
        CreatedBy = request.CreaterId,
        CreatedAtUtc = DateTime.UtcNow,
        Channels = new HashSet<DbChannel> { newChannel },
        Users = workspaceUsers.ToHashSet()
      };
    }
  }
}
