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

        public void Add(DbChannel channel)
        {
            _provider.Channels.Add(channel);
            _provider.Save();
        }
    }
}
