using LT.DigitalOffice.MessageService.Mappers.Models.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Models.Emails;
using System;

namespace LT.DigitalOffice.MessageService.Mappers.Models
{
    class UnsentEmailInfoMapper : IUnsentEmailInfoMapper
    {
        public UnsentEmailInfo Map(DbUnsentEmail email)
        {
            if (email == null)
            {
                throw new ArgumentNullException(nameof(email));
            }

            return new UnsentEmailInfo
            {
                Id = email.Id,
                EmailId = email.EmailId,
                CreatedAt = email.CreatedAt,
                LastSendAt = email.LastSendAt,
                TotalSendingCount = email.TotalSendingCount
            };
        }
    }
}
