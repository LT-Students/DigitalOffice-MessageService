using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.ImageSupport.Helpers.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Db.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.MessageService.Mappers.Db
{
  public class DbChannelMapper : IDbChannelMapper
  {
    private readonly IImageResizeHelper _resizeHelper;
    private readonly IDbChannelUserMapper _channelUserMapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DbChannelMapper(
      IImageResizeHelper resizeHelper,
      IDbChannelUserMapper channelUserMapper,
      IHttpContextAccessor httpContextAccessor)
    {
      _resizeHelper = resizeHelper;
      _channelUserMapper = channelUserMapper;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task<DbChannel> MapAsync(CreateChannelRequest request)
    {
      if (request is null)
      {
        return null;
      }

      Guid createdBy = _httpContextAccessor.HttpContext.GetUserId();
      Guid channelId = Guid.NewGuid();

      List<DbChannelUser> dbChannelUsers = request.UsersIds
        .Select(u => _channelUserMapper.Map(channelId, u, false, createdBy)).ToList();

      dbChannelUsers.Add(_channelUserMapper.Map(channelId, createdBy, true, createdBy));

      (bool _, string resizedContent, string extension) = request.Image is null
        ? (false, null, null)
        : (await _resizeHelper.ResizeAsync(request.Image.Content, request.Image.Extension));

      return new()
      {
        Id = channelId,
        WorkspaceId = request.WorkspaceId,
        Name = request.Name,
        IsPrivate = request.IsPrivate,
        IsActive = true,
        ImageContent = resizedContent,
        ImageExtension = extension,
        CreatedBy = createdBy,
        CreatedAtUtc = DateTime.UtcNow,
        Users = dbChannelUsers.ToHashSet()
      };
    }

    public DbChannel Map(Guid workspaceId, List<DbWorkspaceUser> workspaseUsers, Guid createdBy)
    {
      const string defaultChannelName = "General";

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
        Users = workspaseUsers?.Select(wu =>
          _channelUserMapper.Map(channelId, wu.UserId, wu.IsAdmin, createdBy)).ToHashSet()
      };
    }
  }
}
