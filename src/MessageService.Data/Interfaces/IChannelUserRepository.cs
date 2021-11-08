using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Db;

namespace LT.DigitalOffice.MessageService.Data.Interfaces
{
  [AutoInject]
  public interface IChannelUserRepository
  {
    Task<bool> CreateAsync(List<DbChannelUser> channelUsers);

    Task<DbChannelUser> GetAsync(Guid userId, Guid channelId);

    Task<bool> RemoveAsync(Guid channelId, IEnumerable<Guid> usersIds);

    Task<bool> IsChannelAdminAsync(Guid channelId, Guid userId);
  }
}
