﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LT.DigitalOffice.MessageService.Models.Db
{
  public class DbWorkspace
  {
    public const string TableName = "Workspaces";

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public string AvatarContent { get; set; }
    public string AvatarExtension { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public Guid? ModifiedBy { get; set; }
    public DateTime? ModifiedAtUtc { get; set; }

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
