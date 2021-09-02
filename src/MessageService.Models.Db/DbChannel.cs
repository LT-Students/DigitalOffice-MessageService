using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LT.DigitalOffice.MessageService.Models.Db
{
  public class DbChannel
  {
    public const string TableName = "Channels";

    public Guid Id { get; set; }
    public Guid WorkspaceId { get; set; }
    public string Name { get; set; }
    public bool IsPrivate { get; set; }
    public bool IsActive { get; set; }
    public string AvatarContent { get; set; }
    public string AvatarExtension { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public Guid? ModifiedBy { get; set; }
    public DateTime? ModifiedAtUtc { get; set; }

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
