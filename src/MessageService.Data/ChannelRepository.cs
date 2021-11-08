using System;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Data.Provider;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Channel.Filters;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.MessageService.Data
{
  public class ChannelRepository : IChannelRepository
  {
    private readonly IDataProvider _provider;

    public ChannelRepository(IDataProvider provider)
    {
      _provider = provider;
    }

    public async Task<Guid?> CreateAsync(DbChannel dbChannel)
    {
      if (dbChannel is null)
      {
        return null;
      }

      _provider.Channels.Add(dbChannel);
      await _provider.SaveAsync();

      return dbChannel.Id;
    }

    public async Task<DbChannel> GetAsync(Guid channelId)
    {
      return await _provider.Channels
        .FirstOrDefaultAsync(dbChannel => dbChannel.Id == channelId);
    }

    public async Task<DbChannel> GetAsync(GetChannelFilter filter)
    {
      if (filter is null)
      {
        return null;
      }

      IQueryable<DbChannel> dbChannel = _provider.Channels.AsQueryable();

      if (filter.IncludeUsers)
      {
        dbChannel = dbChannel.Include(c => c.Users);
      }

      return await dbChannel.FirstOrDefaultAsync(w => w.Id == filter.ChannelId);
    }
  }
}
