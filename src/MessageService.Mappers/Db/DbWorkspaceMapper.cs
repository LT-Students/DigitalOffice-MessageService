using System;
using System.Collections.Generic;
using System.Linq;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.MessageService.Mappers.Db.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Helpers.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.MessageService.Mappers.Db
{
  public class DbWorkspaceMapper : IDbWorkspaceMapper
  {
    private readonly IResizeImageHelper _resizeHelper;
    private readonly IDbWorkspaceUserMapper _workspaceUserMapper;
    private readonly IDbChannelMapper _channelMapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DbWorkspaceMapper(
      IResizeImageHelper resizeHelper,
      IDbWorkspaceUserMapper workspaceUserMapper,
      IDbChannelMapper channelMapper,
      IHttpContextAccessor httpContextAccessor)
    {
      _resizeHelper = resizeHelper;
      _workspaceUserMapper = workspaceUserMapper;
      _channelMapper = channelMapper;
      _httpContextAccessor = httpContextAccessor;
    }

    public DbWorkspace Map(CreateWorkspaceRequest request, List<Guid> usersIds)
    {
      if (request == null)
      {
        return null;
      }

      Guid workspaceId = Guid.NewGuid();
      Guid createdBy = _httpContextAccessor.HttpContext.GetUserId();

      ICollection<DbWorkspaceUser> dbWorkspaceUsers = usersIds?
        .Select(ui => _workspaceUserMapper
          .Map(workspaceId, ui, request.Users.FirstOrDefault(u => u.UserId == ui).IsAdmin, createdBy))
        .ToHashSet();

      dbWorkspaceUsers.Add(_workspaceUserMapper.Map(workspaceId, createdBy, true, createdBy));

      return new DbWorkspace
      {
        Id = workspaceId,
        Name = request.Name,
        Description = request.Description,
        IsActive = true,
        ImageContent = request.Image != null ?
          _resizeHelper.Resize(request.Image.Content, request.Image.Extension) : null,
        ImageExtension = request.Image?.Extension,
        CreatedBy = createdBy,
        CreatedAtUtc = DateTime.UtcNow,
        Users = dbWorkspaceUsers.ToHashSet(),
        Channels = new List<DbChannel>()
        {
          _channelMapper.Map(workspaceId, dbWorkspaceUsers.ToList(), createdBy)
        }
      };
    }

    public DbWorkspace Map(string name, List<Guid> usersIds, Guid createdBy)
    {
      Guid workspaceId = Guid.NewGuid();
      string defaultDescription = ""; // TODO Create description for default workspace

      ICollection<DbWorkspaceUser> dbWorkspaceUsers = usersIds?
        .Select(ui => _workspaceUserMapper
          .Map(workspaceId, ui, false, createdBy)).ToList();

      dbWorkspaceUsers?.Add(_workspaceUserMapper.Map(workspaceId, createdBy, true, createdBy));

      return new DbWorkspace
      {
        Id = workspaceId,
        Name = name,
        Description = defaultDescription,
        IsActive = true,
        CreatedBy = createdBy,
        CreatedAtUtc = DateTime.UtcNow,
        Users = dbWorkspaceUsers.ToHashSet(),
        Channels = new List<DbChannel>()
        {
          _channelMapper.Map(workspaceId, dbWorkspaceUsers?.ToList(), createdBy)
        }
      };
    }
  }
}
