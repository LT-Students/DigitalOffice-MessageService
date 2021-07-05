using LT.DigitalOffice.MessageService.Mappers.Models.Interfaces;
using LT.DigitalOffice.MessageService.Models.Dto.Helpers;
using LT.DigitalOffice.Models.Broker.Responses.Company;
using System;

namespace LT.DigitalOffice.MessageService.Mappers.Db
{
    public class SmtpCredentialsMapper : ISmtpCredentialsMapper
    {
        public SmtpCredentials Map(IGetSmtpCredentialsResponse request)
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
