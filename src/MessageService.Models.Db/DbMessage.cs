using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LT.DigitalOffice.MessageService.Models.Db
{
  public class DbMessage
  {
    public const string TableName = "Messages";

    public Guid Id { get; set; }
    public Guid ChannelId { get; set; }
    public string Content { get; set; }
    public int Status { get; set; }
    public int ThreadMessagesCount { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public Guid? ModifiedBy { get; set; }
    public DateTime? ModifiedAtUtc { get; set; }

    public DbChannel Channel { get; set; }

    public ICollection<DbThreadMessage> ThreadMessages { get; set; }
    public ICollection<DbMessageFile> Files { get; set; }
    public ICollection<DbMessageImage> Images { get; set; }

    public DbMessage()
    {
      ThreadMessages = new HashSet<DbThreadMessage>();
      Images = new HashSet<DbMessageImage>();
      Files = new HashSet<DbMessageFile>();
    }
  }

  public class DbMessageConfiguration : IEntityTypeConfiguration<DbMessage>
  {
    public void Configure(EntityTypeBuilder<DbMessage> builder)
    {
      builder
        .ToTable(DbMessage.TableName);

      builder
        .HasKey(w => w.Id);

      builder
        .Property(w => w.Content)
        .IsRequired();

      builder
        .HasOne(m => m.Channel)
        .WithMany(c => c.Messages);

      builder
        .HasMany(m => m.ThreadMessages)
        .WithOne(tm => tm.Message);

      builder
        .HasMany(m => m.Files)
        .WithOne(mf => mf.Message);

      builder
        .HasMany(m => m.Images)
        .WithOne(mf => mf.Message);
    }
  }
}
