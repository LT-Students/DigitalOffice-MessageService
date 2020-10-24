using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.MessageService.Mappers.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using System;

namespace LT.DigitalOffice.MessageService.Mappers
{
    public class EmailMapper : IMapper<ISendEmailRequest, DbEmail>
    {
        public DbEmail Map(ISendEmailRequest email)
        {
            if (email == null)
            {
                throw new ArgumentNullException(nameof(email));
            }

            return new DbEmail
            {
                Id = Guid.NewGuid(),
                SenderId = email.SenderId,
                Receiver = email.Receiver,
                Time = DateTime.UtcNow,
                Subject = email.Subject,
                Body = email.Body
            };
        }
    }
}
