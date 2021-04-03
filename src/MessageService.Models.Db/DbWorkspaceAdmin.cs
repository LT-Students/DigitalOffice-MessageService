using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace LT.DigitalOffice.MessageService.Models.Db
{
    public class DbWorkspaceAdmin
    {
        public const string TableName = "WorkspaceAdmins";

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid WorkspaceId { get; set; }

        public DbWorkspace Workspace { get; set; }
    }

    public class DbWorkspaceAdminConfiguration : IEntityTypeConfiguration<DbWorkspaceAdmin>
    {
        public void Configure(EntityTypeBuilder<DbWorkspaceAdmin> builder)
        {
            builder
                .ToTable(DbWorkspaceAdmin.TableName);

            builder
                .HasKey(wa => wa.Id);

            builder
                .Property(wa => wa.UserId)
                .IsRequired();

            builder
                .Property(wa => wa.WorkspaceId)
                .IsRequired();

            builder
                .HasOne(wa => wa.Workspace)
                .WithMany(w => w.Admins)
                .HasForeignKey(wa => wa.WorkspaceId);
        }
    }
}
