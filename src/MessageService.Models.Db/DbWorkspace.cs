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

        public ICollection<DbWorkspaceAdmin> Admins { get; set; }
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
                .HasMany(w => w.Admins)
                .WithOne(wa => wa.Workspace);
        }
    }
}
