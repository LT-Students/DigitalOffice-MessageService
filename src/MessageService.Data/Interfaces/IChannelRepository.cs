using System;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Filtres;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.MessageService.Data.Interfaces
{
  [AutoInject]
  public interface IChannelRepository
  {
    Task<Guid?> CreateAsync(DbChannel dbChannel);

    Task<DbChannel> GetAsync(Guid channelId);

    Task<DbChannel> GetAsync(Guid channelId, GetChannelFilter filter);

    Task<bool> EditAsync(DbChannel channel, JsonPatchDocument<DbChannel> document);
  }
}
