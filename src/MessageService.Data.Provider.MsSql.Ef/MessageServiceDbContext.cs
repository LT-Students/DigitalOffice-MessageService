using System.Reflection;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.EFSupport.Provider;
using LT.DigitalOffice.MessageService.Models.Db;
using Microsoft.EntityFrameworkCore;

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

    public DbSet<DbWorkspace> Workspaces { get; set; }
    public DbSet<DbWorkspaceUser> WorkspacesUsers { get; set; }
    public DbSet<DbChannel> Channels { get; set; }
    public DbSet<DbChannelUser> ChannelsUsers { get; set; }
    public DbSet<DbMessage> Messages { get; set; }
    public DbSet<DbThreadMessage> ThreadsMessages { get; set; }
    public DbSet<DbMessageFile> MessagesFiles { get; set; }
    public DbSet<DbMessageImage> MessagesImages { get; set; }

    void IBaseDataProvider.Save()
    {
      SaveChanges();
    }

    async Task IBaseDataProvider.SaveAsync()
    {
      await SaveChangesAsync();
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
