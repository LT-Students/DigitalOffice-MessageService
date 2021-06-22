using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.MessageService.Models.Db
{
    public class DbWorkspaceUser
    {
        public const string TableName = "WorkspaceUsers";

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid WorkspaceId { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeactivatedAt { get; set; }


        public DbWorkspace Workspace { get; set; }
        public ICollection<DbChannelUser> ChannelsUsers { get; set; }
    }

    public class DbWorkspaceUsersConfiguration : IEntityTypeConfiguration<DbWorkspaceUser>
    {
        public void Configure(EntityTypeBuilder<DbWorkspaceUser> builder)
        {
            builder
                .ToTable(DbWorkspaceUser.TableName);

            builder
                .HasKey(wu => wu.Id);

            builder
                .Property(wu => wu.UserId)
                .IsRequired();

            builder
                .Property(wu => wu.WorkspaceId)
                .IsRequired();

            builder
                .HasOne(wu => wu.Workspace)
                .WithMany(w => w.Users)
                .HasForeignKey(wu => wu.WorkspaceId);

            builder
                .HasMany(wu => wu.ChannelsUsers)
                .WithOne(chu => chu.WorkspaceUser);
        }
    }
}
