using LT.DigitalOffice.MessageService.Mappers.Models.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Models.Emails;
using System;

namespace LT.DigitalOffice.MessageService.Mappers.Models
{
    public class EmailInfoMapper : IEmailInfoMapper
    {
        public EmailInfo Map(DbEmail email)
        {
            if (email == null)
            {
                throw new ArgumentNullException(nameof(email));
            }

            return new EmailInfo
            {
                Id = email.Id,
                Body = email.Body,
                Subject = email.Subject,
                Receiver = email.Receiver
            };
        }
    }
}
