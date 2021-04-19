using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.MessageService.Mappers.Db.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using System;

namespace LT.DigitalOffice.MessageService.Mappers.Db
{
    public class DbEmailMapper : IDbEmailMapper
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
