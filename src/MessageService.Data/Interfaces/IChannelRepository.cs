using System;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Channel.Filters;

namespace LT.DigitalOffice.MessageService.Data.Interfaces
{
  [AutoInject]
  public interface IChannelRepository
  {
    Task<Guid?> CreateAsync(DbChannel dbChannel);

    Task<DbChannel> GetAsync(Guid channelId);

    Task<DbChannel> GetAsync(GetChannelFilter filter);
  }
}
