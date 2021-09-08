using System;
using System.Collections.Generic;
using System.Linq;
using LT.DigitalOffice.MessageService.Mappers.Db.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Helpers.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests;

namespace LT.DigitalOffice.MessageService.Mappers.Db
{
  public class DbChannelMapper : IDbChannelMapper
  {
    private readonly IResizeImageHelper _resizeHelper;
    private readonly IDbChannelUserMapper _channelUserMapper;

    public DbChannelMapper(
      IResizeImageHelper resizeHelper,
      IDbChannelUserMapper channelUserMapper)
    {
      _resizeHelper = resizeHelper;
      _channelUserMapper = channelUserMapper;
    }

    public DbChannel Map(CreateChannelRequest request, DbWorkspaceUser creatorWorkspaceUser)
    {
      if (request == null)
      {
        return null;
      }

      Guid channelId = Guid.NewGuid();

      ICollection<DbChannelUser> dbChannelUsers = request.Users?.Select(u =>
        _channelUserMapper.Map(channelId, u, creatorWorkspaceUser.UserId)).ToList();

      dbChannelUsers.Add(_channelUserMapper.Map(channelId, creatorWorkspaceUser.Id, true, creatorWorkspaceUser.UserId));

      return new()
      {
        Id = channelId,
        WorkspaceId = request.WorkspaceId,
        Name = request.Name,
        IsPrivate = request.IsPrivate,
        IsActive = true,
        ImageContent = request.Image != null ?
          _resizeHelper.Resize(request.Image.Content, request.Image.Extension) : null,
        ImageExtension = request.Image?.Extension,
        CreatedBy = creatorWorkspaceUser.UserId,
        CreatedAtUtc = DateTime.UtcNow,
        Users = dbChannelUsers.ToHashSet()
      };
    }

    public DbChannel Map(Guid workspaceId, List<DbWorkspaceUser> users, Guid createdBy)
    {
      string defaultChannelName = "General";

      Guid channelId = Guid.NewGuid();

      return new DbChannel()
      {
        Id = channelId,
        WorkspaceId = workspaceId,
        Name = defaultChannelName,
        IsPrivate = false,
        IsActive = true,
        CreatedBy = createdBy,
        CreatedAtUtc = DateTime.UtcNow,
        Users = users?.Select(u =>
          _channelUserMapper.Map(channelId, u.Id, u.IsAdmin, createdBy)).ToHashSet()
      };
    }
  }
}
