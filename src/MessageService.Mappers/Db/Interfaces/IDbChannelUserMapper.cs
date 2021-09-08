using System;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Channel;

namespace LT.DigitalOffice.MessageService.Mappers.Db.Interfaces
{
  [AutoInject]
  public interface IDbChannelUserMapper
  {
    DbChannelUser Map(Guid channelId, ChannelUserRequest request, Guid createdBy);

    DbChannelUser Map(Guid channelId, Guid workspaceUserId, bool isAdmin, Guid createdBy);
  }
}
