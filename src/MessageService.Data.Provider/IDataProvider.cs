using LT.DigitalOffice.MessageService.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.MessageService.Data.Provider
{
    public interface IDataProvider
    {
        DbSet<DbMessage> Messages { get; set; }
        DbSet<DbEmail> Emails { get; set; }

        void Save();
        void EnsureDeleted();
        bool IsInMemory();
    }
}