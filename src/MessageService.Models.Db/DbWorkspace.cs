using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.MessageService.Models.Db
{
    public class DbWorkspace
    {
        public const string TableName = "Workspaces";

        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public Guid? ImageId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeactivatedAt { get; set; }

        public ICollection<DbWorkspaceUser> Users { get; set; }
        public ICollection<DbChannel> Channels { get; set; }

        public DbWorkspace()
        {
            Users = new HashSet<DbWorkspaceUser>();
            Channels = new HashSet<DbChannel>();
        }
    }

    public class DbWorkspaceConfiguration : IEntityTypeConfiguration<DbWorkspace>
    {
        public void Configure(EntityTypeBuilder<DbWorkspace> builder)
        {
            builder
                .ToTable(DbWorkspace.TableName);

            builder
                .HasKey(w => w.Id);

            builder
                .Property(w => w.Name)
                .IsRequired();

            builder
                .HasMany(w => w.Users)
                .WithOne(wa => wa.Workspace);

            builder
                .HasMany(w => w.Channels)
                .WithOne(ch => ch.Workspace);
        }
    }
}
