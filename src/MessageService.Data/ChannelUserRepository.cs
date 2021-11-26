using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Data.Provider;
using LT.DigitalOffice.MessageService.Models.Db;

namespace LT.DigitalOffice.MessageService.Data
{
  public class ChannelUserRepository : IChannelUserRepository
  {
    private readonly IDataProvider _provider;

    public ChannelUserRepository(IDataProvider provider)
    {
      _provider = provider;
    }

    public async Task CreateAsync(IEnumerable<DbChannelUser> dbChannelUsers)
    {
      _provider.ChannelsUsers.AddRange(dbChannelUsers);
      await _provider.SaveAsync();
    }
  }
}
