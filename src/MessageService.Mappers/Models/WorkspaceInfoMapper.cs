using System;
using System.Collections.Generic;
using System.Linq;
using LT.DigitalOffice.MessageService.Mappers.Models.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Models;
using LT.DigitalOffice.Models.Broker.Models;

namespace LT.DigitalOffice.MessageService.Mappers.Models
{
  public class WorkspaceInfoMapper : IWorkspaceInfoMapper
  {
    private readonly IShortChannelInfoMapper _channelInfoMapper;
    private readonly IUserInfoMapper _userInfoMapper;

    public WorkspaceInfoMapper(
      IShortChannelInfoMapper channelInfoMapper,
      IUserInfoMapper userInfoMapper)
    {
      _channelInfoMapper = channelInfoMapper;
      _userInfoMapper = userInfoMapper;
    }

    public WorkspaceInfo Map(DbWorkspace workspace, List<ImageInfo> images, List<UserData> users)
    {
      if (workspace == null)
      {
        return null;
      }

      UserData user = users?.FirstOrDefault(u => u.Id == workspace.CreatedBy);

      return new WorkspaceInfo
      {
        Id = workspace.Id,
        Name = workspace.Name,
        Image = images?.FirstOrDefault(i => i.Id == workspace.ImageId),
        Description = workspace.Description,
        CreatedAtUtc = workspace.CreatedAtUtc,
        CreatedBy = _userInfoMapper.Map(user, images?.FirstOrDefault(i => i.Id == user.ImageId)),
        IsActive = workspace.IsActive,
        Channels = workspace.Channels?.Select(ch => _channelInfoMapper.Map(ch, images?.FirstOrDefault(i => i.Id == ch.ImageId))).ToList(),
        Users = users?
          .Where(u => workspace.Users.Any(wu => wu.UserId == u.Id))
          .Select(u => _userInfoMapper.Map(u, images?.FirstOrDefault(i => i.Id == u.ImageId)))
          .ToList()
      };
    }
  }
}
