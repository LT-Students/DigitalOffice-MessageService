using LT.DigitalOffice.MessageService.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.MessageService.Data.Provider
{
    public interface IDataProvider
    {
        DbSet<DbMessage> Messages { get; set; }
        DbSet<DbEmail> Emails { get; set; }
        DbSet<DbWorkspace> Workspaces { get; set; }
        DbSet<DbEmailTemplate> EmailTemplates { get; set; }

        void Save();
        object MakeEntityDetached(object obj);
        void EnsureDeleted();
        bool IsInMemory();
    }
}