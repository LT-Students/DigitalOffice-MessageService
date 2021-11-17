using System;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Db;

namespace LT.DigitalOffice.MessageService.Mappers.Db.Interfaces
{
  [AutoInject]
  public interface IDbChannelUserMapper
  {
    DbChannelUser Map(Guid channelId, Guid workspaceUserId, bool isAdmin, Guid createdBy);
  }
}
