using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LT.DigitalOffice.MessageService.Models.Db
{
  public class DbWorkspaceUser
  {
    public const string TableName = "WorkspacesUsers";

    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid WorkspaceId { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsActive { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public Guid? ModifiedBy { get; set; }
    public DateTime? ModifiedAtUtc { get; set; }

    public DbWorkspace Workspace { get; set; }

    public ICollection<DbChannelUser> ChannelsUsers { get; set; }

    public DbWorkspaceUser()
    {
      ChannelsUsers = new HashSet<DbChannelUser>();
    }
  }

  public class DbWorkspaceUserConfiguration : IEntityTypeConfiguration<DbWorkspaceUser>
  {
    public void Configure(EntityTypeBuilder<DbWorkspaceUser> builder)
    {
      builder
        .ToTable(DbWorkspaceUser.TableName);

      builder
        .HasKey(wu => wu.Id);

      builder
        .Property(wu => wu.WorkspaceId)
        .IsRequired();

      builder
        .HasOne(wu => wu.Workspace)
        .WithMany(w => w.Users);

      builder
        .HasMany(wu => wu.ChannelsUsers)
        .WithOne(chu => chu.WorkspaceUser);
    }
  }
}
