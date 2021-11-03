using System;
using System.Threading.Tasks;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Data.Provider;
using LT.DigitalOffice.MessageService.Models.Db;
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
      return await _provider.Channels.FirstOrDefaultAsync(dbChannel => dbChannel.Id == channelId);
    }
  }
}
