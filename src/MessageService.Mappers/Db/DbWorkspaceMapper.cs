using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.ImageSupport.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.MessageService.Mappers.Db.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.MessageService.Mappers.Db
{
  public class DbWorkspaceMapper : IDbWorkspaceMapper
  {
    private readonly IImageResizeHelper _resizeHelper;
    private readonly IDbWorkspaceUserMapper _workspaceUserMapper;
    private readonly IDbChannelMapper _channelMapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DbWorkspaceMapper(
      IImageResizeHelper resizeHelper,
      IDbWorkspaceUserMapper workspaceUserMapper,
      IDbChannelMapper channelMapper,
      IHttpContextAccessor httpContextAccessor)
    {
      _resizeHelper = resizeHelper;
      _workspaceUserMapper = workspaceUserMapper;
      _channelMapper = channelMapper;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task<DbWorkspace> MapAsync(CreateWorkspaceRequest request)
    {
      if (request is null)
      {
        return null;
      }

      Guid workspaceId = Guid.NewGuid();
      Guid createdBy = _httpContextAccessor.HttpContext.GetUserId();

      List<DbWorkspaceUser> dbWorkspaceUsers = request.Users?
        .Select(u => _workspaceUserMapper.Map(workspaceId, u, false, createdBy))
        .ToList();

      dbWorkspaceUsers.Add(_workspaceUserMapper.Map(workspaceId, createdBy, true, createdBy));

      (bool _, string resizedContent, string extension) = request.Image is null
        ? (false, null, null)
        : (await _resizeHelper.ResizeAsync(request.Image.Content, request.Image.Extension));

      return new DbWorkspace
      {
        Id = workspaceId,
        Name = request.Name,
        Description = request.Description,
        IsActive = true,
        ImageContent = resizedContent,
        ImageExtension = extension,
        CreatedBy = createdBy,
        CreatedAtUtc = DateTime.UtcNow,
        Users = dbWorkspaceUsers,
        Channels = new List<DbChannel>()
        {
          _channelMapper.Map(workspaceId, dbWorkspaceUsers, createdBy)
        }
      };
    }

    public DbWorkspace Map(string name, List<Guid> usersIds, Guid createdBy)
    {
      Guid workspaceId = Guid.NewGuid();
      const string defaultDescription = ""; // TODO Create description for default workspace

      List<DbWorkspaceUser> dbWorkspaceUsers = usersIds?
        .Select(ui => _workspaceUserMapper
          .Map(workspaceId, ui, ui == createdBy, createdBy)).ToList();

      return new DbWorkspace
      {
        Id = workspaceId,
        Name = name,
        Description = defaultDescription,
        IsActive = true,
        CreatedBy = createdBy,
        CreatedAtUtc = DateTime.UtcNow,
        Users = dbWorkspaceUsers,
        Channels = new List<DbChannel>()
        {
          _channelMapper.Map(workspaceId, dbWorkspaceUsers, createdBy)
        }
      };
    }
  }
}
