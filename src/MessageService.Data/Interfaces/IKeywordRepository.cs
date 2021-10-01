using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Db;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.MessageService.Data.Interfaces
{
    [AutoInject]
    public interface IKeywordRepository
    {
        List<DbKeyword> Find(int skipCount, int takeCount, out int totalCount);

        DbKeyword Get(Guid entityId);

        Guid Add(DbKeyword entity);

        bool Remove(Guid entityId);

        bool DoesKeywordExist(string keyword);
    }
}
