using System.Collections.Generic;
using LT.DigitalOffice.MessageService.Mappers.Models.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Models.Image;
using LT.DigitalOffice.MessageService.Models.Dto.Models.User;
using LT.DigitalOffice.MessageService.Models.Dto.Responses.Channel;
using LT.DigitalOffice.MessageService.Models.Dto.Responses.Message;

namespace LT.DigitalOffice.MessageService.Mappers.Models
{
  public class ChannelInfoMapper : IChannelInfoMapper
  {
    public ChannelInfo Map(DbChannel dbChannel, List<MessageInfo> messages, List<UserInfo> users)
    {
      if (dbChannel is null)
      {
        return null;
      }

      ImageConsist image = dbChannel.ImageContent is null
        ? null
        : new() { Content = dbChannel.ImageContent, Extension = dbChannel.ImageExtension };

      return new()
      {
        Id = dbChannel.Id,
        Avatar = image,
        Name = dbChannel.Name,
        IsPrivate = dbChannel.IsPrivate,
        Messages = messages,
        Users = users
      };
    }
  }
}
