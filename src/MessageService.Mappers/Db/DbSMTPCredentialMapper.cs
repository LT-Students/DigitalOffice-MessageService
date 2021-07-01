using LT.DigitalOffice.MessageService.Mappers.Db.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.Models.Broker.Requests.Message;
using System;

namespace LT.DigitalOffice.MessageService.Mappers.Db
{
    public class DbSMTPCredentialMapper : IDbSMTPCredentialMapper
    {
        public DbSMTPCredential Map(ICreateSMTPRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return new()
            {
                Host = request.Host,
                Port = request.Port,
                EnableSsl = request.EnableSsl,
                Email = request.Email,
                Password = request.Password
            };
        }
    }
}
