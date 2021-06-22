using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.MessageService.Models.Db
{
    public class DbChannel
    {
        public const string TableName = "Channels";

		public Guid Id { get; set; }
		public Guid WorkspaceId { get; set; }
        public Guid OwnerId { get; set; }
        public Guid? ImageId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsPrivate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DeactivatedAt { get; set; }


        public ICollection<DbChannelUser> Users { get; set; }
		public DbWorkspace Workspace { get; set; }

        public DbChannel()
        {
            Users = new HashSet<DbChannelUser>();
        }
    }

    public class DbChannelConfiguration : IEntityTypeConfiguration<DbChannel>
    {
        public void Configure(EntityTypeBuilder<DbChannel> builder)
        {
            builder
                .ToTable(DbChannel.TableName);

            builder
                .HasKey(ch => ch.Id);

            builder
                .Property(ch => ch.Name)
                .IsRequired();

            builder
                .HasOne(ch => ch.Workspace)
                .WithMany(w => w.Channels)
                .HasForeignKey(ch => ch.WorkspaceId);

            builder
                .HasMany(ch => ch.Users)
                .WithOne(chu => chu.Channel);
        }
    }
}
