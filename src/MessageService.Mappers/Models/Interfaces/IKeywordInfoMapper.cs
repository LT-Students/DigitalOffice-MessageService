﻿using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Models;

namespace LT.DigitalOffice.MessageService.Mappers.Models.Interfaces
{
    [AutoInject]
    public interface IKeywordInfoMapper
    {
        KeywordInfo Map(DbKeyword entity);
    }
}