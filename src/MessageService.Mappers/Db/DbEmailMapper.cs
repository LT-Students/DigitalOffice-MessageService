using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.MessageService.Mappers.Db.Email.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.Models.Broker.Requests.Message;
using System;

namespace LT.DigitalOffice.MessageService.Mappers.Db.Email
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
                CreatedAtUtc = DateTime.UtcNow
            };
        }
    }
}
