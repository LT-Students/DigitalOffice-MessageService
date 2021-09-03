using System.Collections.Generic;
using System.Linq;
using LT.DigitalOffice.MessageService.Mappers.Models.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Models;
using LT.DigitalOffice.MessageService.Models.Dto.Models.Workspace;
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

    public WorkspaceInfo Map(DbWorkspace dbWorkspace, List<ImageInfo> images, List<UserData> users)
    {
      if (dbWorkspace == null)
      {
        return null;
      }

      UserData user = users?.FirstOrDefault(u => u.Id == dbWorkspace.CreatedBy);

      return new WorkspaceInfo
      {
        Id = dbWorkspace.Id,
        Name = dbWorkspace.Name,
        Description = dbWorkspace.Description,
        Image = new Image()
        {
          Content = dbWorkspace.ImageContent,
          Extension = dbWorkspace.ImageExtension
        },
        CreatedAtUtc = dbWorkspace.CreatedAtUtc,
        CreatedBy = _userInfoMapper
          .Map(user, images?.FirstOrDefault(i => i.Id == user.ImageId)),
        IsActive = dbWorkspace.IsActive,
        Channels = dbWorkspace.Channels?
          .Select(ch => _channelInfoMapper.Map(ch)).ToList(),
        Users = users?
          .Where(u => dbWorkspace.Users.Any(wu => wu.UserId == u.Id))
          .Select(u => _userInfoMapper.Map(u, images?.FirstOrDefault(i => i.Id == u.ImageId)))
          .ToList()
      };
    }
  }
}
