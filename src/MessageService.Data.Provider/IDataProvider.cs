using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Database;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.MessageService.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.MessageService.Data.Provider
{
  [AutoInject(InjectType.Scoped)]
  public interface IDataProvider : IBaseDataProvider
  {
    DbSet<DbWorkspace> Workspaces { get; set; }
    DbSet<DbWorkspaceUser> WorkspacesUsers { get; set; }
    DbSet<DbChannel> Channels { get; set; }
    DbSet<DbChannelUser> ChannelsUsers { get; set; }
    DbSet<DbMessage> Messages { get; set; }
    DbSet<DbThreadMessage> ThreadsMessages { get; set; }
    DbSet<DbMessageFile> MessagesFiles { get; set; }
    DbSet<DbMessageFile> MessagesImages { get; set; }
  }
}
