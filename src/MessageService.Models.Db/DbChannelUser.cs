using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LT.DigitalOffice.MessageService.Models.Db
{
  public class DbChannelUser
  {
    public const string TableName = "ChannelsUsers";

    public Guid Id { get; set; }
    public Guid UserId { get; set; }
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
        .HasKey(cu => cu.Id);

      builder
        .HasOne(cu => cu.Channel)
        .WithMany(c => c.Users);

      builder
        .HasOne(cu => cu.WorkspaceUser)
        .WithMany(wu => wu.ChannelsUsers);
    }
  }
}
