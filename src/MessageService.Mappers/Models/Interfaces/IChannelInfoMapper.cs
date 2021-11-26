using System.Collections.Generic;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Models.User;
using LT.DigitalOffice.MessageService.Models.Dto.Responses.Channel;
using LT.DigitalOffice.MessageService.Models.Dto.Responses.Message;

namespace LT.DigitalOffice.MessageService.Mappers.Models.Interfaces
{
  [AutoInject]
  public interface IChannelInfoMapper
  {
    ChannelInfo Map(DbChannel dbChannel, List<MessageInfo> messages, List<UserInfo> users);
  }
}
