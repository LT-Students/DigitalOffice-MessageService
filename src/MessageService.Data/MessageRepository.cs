using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Data.Provider;

namespace LT.DigitalOffice.MessageService.Data
{
    public class MessageRepository : IMessageRepository
    {
        private readonly IDataProvider provider;

        public MessageRepository(IDataProvider provider)
        {
            this.provider = provider;
        }
    }
}