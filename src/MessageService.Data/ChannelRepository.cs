using System;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Data.Provider;
using LT.DigitalOffice.MessageService.Models.Db;

namespace LT.DigitalOffice.MessageService.Data
{
  public class ChannelRepository : IChannelRepository
  {
    private readonly IDataProvider _provider;

    public ChannelRepository(IDataProvider provider)
    {
      _provider = provider;
    }

    public Guid? Add(DbChannel dbChannel)
    {
      if (dbChannel == null)
      {
        return null;
      }

      _provider.Channels.Add(dbChannel);
      _provider.Save();

      return dbChannel.Id;
    }
  }
}
