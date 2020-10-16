using LT.DigitalOffice.MessageService.Models.Db;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace LT.DigitalOffice.MessageService.Data.Provider.MsSql.Ef
{
    /// <summary>
    /// A class that defines the tables and its properties in the database of MessageService.
    /// </summary>
    public class MessageServiceDbContext : DbContext, IDataProvider
    {
        public MessageServiceDbContext(DbContextOptions<MessageServiceDbContext> options)
            : base(options)
        {
        }

        public DbSet<DbMessage> Messages { get; set; }
        public DbSet<DbEmail> Emails { get; set; }
        public DbSet<DbEmailTemplate> EmailTemplates { get; set; }

        void IDataProvider.Save()
        {
            SaveChanges();
        }

        public void EnsureDeleted()
        {
            Database.EnsureDeleted();
        }

        public object MakeEntityDetached(object obj)
        {
            Entry(obj).State = EntityState.Detached;
            return Entry(obj).State;
        }

        public bool IsInMemory()
        {
            return Database.IsInMemory();
        }

        // Fluent API is written here.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("LT.DigitalOffice.MessageService.Models.Db"));
        }
    }
}
