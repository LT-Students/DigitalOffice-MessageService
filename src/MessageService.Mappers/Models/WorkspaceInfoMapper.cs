using System.Collections.Generic;
using System.Linq;
using LT.DigitalOffice.MessageService.Mappers.Models.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Models;
using LT.DigitalOffice.MessageService.Models.Dto.Models.Image;
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

    public WorkspaceInfo Map(DbWorkspace dbWorkspace, List<UserData> users)
    {
      if (dbWorkspace == null)
      {
        return null;
      }

      UserData creatorUserData = users?.FirstOrDefault(u => u.Id == dbWorkspace.CreatedBy);

      return new WorkspaceInfo
      {
        Id = dbWorkspace.Id,
        Name = dbWorkspace.Name,
        Description = dbWorkspace.Description,
        Image = new ImageConsist()
        {
          Content = dbWorkspace.ImageContent,
          Extension = dbWorkspace.ImageExtension
        },
        CreatedAtUtc = dbWorkspace.CreatedAtUtc,
        CreatedBy = _userInfoMapper
          .Map(creatorUserData),
        IsActive = dbWorkspace.IsActive,
        Channels = dbWorkspace.Channels?
          .Select(_channelInfoMapper.Map).ToList(),
        Users = users?
          .Where(u => dbWorkspace.Users.Any(wu => wu.UserId == u.Id))
          .Select(u => _userInfoMapper.Map(u))
          .ToList()
      };
    }
  }
}
