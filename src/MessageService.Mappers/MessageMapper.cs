using LT.DigitalOffice.MessageService.Mappers.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto;
using System;

namespace LT.DigitalOffice.MessageService.Mappers
{
    public class MessageMapper : IMapper<Message, DbMessage>
    {
        public DbMessage Map(Message message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            return new DbMessage
            {
                Id = Guid.NewGuid(),
                Title = message.Title,
                Content = message.Content,
                Status = 0,
                SenderUserId = message.SenderUserId
            };
        }
    }
}
