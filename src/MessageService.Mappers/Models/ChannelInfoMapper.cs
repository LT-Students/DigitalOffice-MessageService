using System.Collections.Generic;
using System.Linq;
using LT.DigitalOffice.MessageService.Mappers.Models.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Models.Image;
using LT.DigitalOffice.MessageService.Models.Dto.Responses.Channel;
using LT.DigitalOffice.Models.Broker.Models;

namespace LT.DigitalOffice.MessageService.Mappers.Models
{
  public class ChannelInfoMapper : IChannelInfoMapper
  {
    private readonly IUserInfoMapper _userInfoMapper;

    public ChannelInfoMapper(
      IUserInfoMapper userInfoMapper)
    {
      _userInfoMapper = userInfoMapper;
    }

    public ChannelInfo Map(DbChannel dbChannel, List<UserData> users)
    {
      if (dbChannel is null)
      {
        return null;
      }

      return new ChannelInfo
      {
        Id = dbChannel.Id,
        Avatar = new ImageConsist()
        {
          Content = dbChannel.ImageContent,
          Extension = dbChannel.ImageExtension
        },
        Name = dbChannel.Name,
        IsActive = dbChannel.IsActive,
        IsPrivate = dbChannel.IsPrivate,
        Users = users?
          .Where(u => dbChannel.Users.Any(cu => cu.WorkspaceUserId == u.Id))
          .Select(_userInfoMapper.Map).ToList()
      };
    }
  }
}
