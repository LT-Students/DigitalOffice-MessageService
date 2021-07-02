using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Db;
using System.Collections.Generic;

namespace LT.DigitalOffice.MessageService.Data.Interfaces
{
    [AutoInject]
    public interface IChannelUserRepository
    {
        void AddRange(IEnumerable<DbChannelUser> channelUsers);
    }
}
