using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Db;

namespace LT.DigitalOffice.MessageService.Data.Interfaces
{
    [AutoInject]
    public interface IChannelRepository
    {
        void Add(DbChannel channel);
    }
}
