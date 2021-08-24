using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Db;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.MessageService.Data.Interfaces
{
    [AutoInject]
    public interface IUnsentEmailRepository
    {
        IEnumerable<DbUnsentEmail> GetAll(int totalSendingCountIsLessThen);

        IEnumerable<DbUnsentEmail> Find(int skipCount, int takeCount, out int totalCount);

        void Add(DbUnsentEmail email);

        DbUnsentEmail Get(Guid id);

        bool Remove(DbUnsentEmail email);

        void IncrementTotalCount(DbUnsentEmail email);
    }
}
