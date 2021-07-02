using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Data.Provider;
using LT.DigitalOffice.MessageService.Models.Db;
using System.Collections.Generic;

namespace LT.DigitalOffice.MessageService.Data
{
    public class ChannelUserRepository : IChannelUserRepository
    {
        private readonly IDataProvider _provider;

        public ChannelUserRepository(IDataProvider provider)
        {
            _provider = provider;
        }

        public void AddRange(IEnumerable<DbChannelUser> channelUsers)
        {
            _provider.ChannelUsers.AddRange(channelUsers);
            _provider.Save();
        }
    }
}
