using LT.DigitalOffice.MessageService.Mappers.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto;
using System;

namespace LT.DigitalOffice.MessageService.Mappers
{
    public class EmailMapper : IMapper<Email, DbEmail>
    {
        public DbEmail Map(Email email)
        {
            if (email == null)
            {
                throw new ArgumentNullException(nameof(email));
            }

            return new DbEmail
            {
                Id = Guid.NewGuid(),
                Receiver = email.Receiver,
                Time = DateTime.UtcNow,
                Subject = email.Subject,
                Body = email.Body
            };
        }
    }
}
