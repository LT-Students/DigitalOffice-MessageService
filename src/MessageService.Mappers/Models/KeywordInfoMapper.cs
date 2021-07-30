using LT.DigitalOffice.MessageService.Mappers.Models.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Enums;
using LT.DigitalOffice.MessageService.Models.Dto.Models;
using System;

namespace LT.DigitalOffice.MessageService.Mappers.Models
{
    public class KeywordInfoMapper : IKeywordInfoMapper
    {
        public KeywordInfo Map(DbKeyword entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return new KeywordInfo
            {
                Id = entity.Id,
                Keyword = entity.Keyword,
                ServiceName = (ServiceName)entity.ServiceName,
                EntityName = entity.EntityName.StartsWith("db", StringComparison.OrdinalIgnoreCase) ? entity.EntityName[2..] : entity.EntityName,
                PropertyName = entity.PropertyName
            };
        }
    }
}
