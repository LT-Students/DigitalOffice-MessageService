using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.MessageService.Mappers.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using System;

namespace LT.DigitalOffice.MessageService.Mappers.EmailMappers
{
    public class EmailMapper : IMapper<ISendEmailRequest, DbEmail>
    {
        public DbEmail Map(
            ISendEmailRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return new DbEmail
            {
                Id = Guid.NewGuid(),
                SenderId = request.SenderId,
                Receiver = request.Email,
                Time = DateTime.UtcNow
            };
        }
    }
}
