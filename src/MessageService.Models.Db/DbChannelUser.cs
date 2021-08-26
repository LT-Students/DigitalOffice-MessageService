using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace LT.DigitalOffice.MessageService.Models.Db
{
    public class DbChannelUser
    {
        public const string TableName = "ChannelUsers";

        public Guid Id { get; set; }
        public Guid WorkspaceUserId { get; set; }
        public Guid ChannelId { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedAtUtc { get; set; }

        public DbWorkspaceUser WorkspaceUser { get; set; }
		public DbChannel Channel { get; set; }
	}

    public class DbChannelUserConfiguration : IEntityTypeConfiguration<DbChannelUser>
    {
        public void Configure(EntityTypeBuilder<DbChannelUser> builder)
        {
            builder
                .ToTable(DbChannelUser.TableName);

            builder
                .HasKey(chu => chu.Id);

            builder
                .HasOne(chu => chu.Channel)
                .WithMany(ch => ch.Users)
                .HasForeignKey(chu => chu.ChannelId);

            builder
                .HasOne(chu => chu.WorkspaceUser)
                .WithMany(wu => wu.ChannelsUsers)
                .HasForeignKey(chu => chu.WorkspaceUserId);
        }
    }
}
