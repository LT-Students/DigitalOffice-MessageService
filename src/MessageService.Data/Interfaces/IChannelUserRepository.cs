using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Db;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.MessageService.Data.Interfaces
{
  [AutoInject]
  public interface IChannelUserRepository
  {
    Task CreateAsync(List<DbChannelUser> channelUsers);

    Task RemoveAsync(Guid workspaceId, List<Guid> usersIds);

    Task RemoveAsync(Guid channelId, List<DbChannelUser> users);

    Task<List<DbChannelUser>> GetByChannelIdAsync(Guid channelId);

    Task<bool> EditAsync(Guid channelId, Guid userId, JsonPatchDocument<DbChannelUser> document);

    Task<bool> ChannelUserExistAsync(Guid channelId, Guid userId);

    Task<bool> IsAdminAsync(Guid channelId, Guid userId);
  }
}
