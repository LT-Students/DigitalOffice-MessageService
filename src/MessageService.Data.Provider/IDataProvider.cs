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
        DbSet<DbMessage> Messages { get; set; }
        DbSet<DbEmail> Emails { get; set; }
        DbSet<DbWorkspace> Workspaces { get; set; }
        DbSet<DbWorkspaceAdmin> WorkspaceAdmins { get; set; }
        DbSet<DbEmailTemplate> EmailTemplates { get; set; }
    }
}