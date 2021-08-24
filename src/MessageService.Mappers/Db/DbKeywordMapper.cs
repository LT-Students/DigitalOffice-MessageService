using LT.DigitalOffice.MessageService.Mappers.Db.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.ParseEntity;
using System;

namespace LT.DigitalOffice.MessageService.Mappers.Db
{
    public class DbKeywordMapper : IDbKeywordMapper
    {
        public DbKeyword Map(AddKeywordRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return new DbKeyword
            {
                Id = Guid.NewGuid(),
                Keyword = request.Keyword,
                ServiceName = (int)request.ServiceName,
                EntityName = "Db" + request.EntityName,
                PropertyName = request.PropertyName
            };
        }
    }
}
