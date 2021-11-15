using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LT.DigitalOffice.MessageService.Models.Db
{
  public class DbMessageFile
  {
    public const string TableName = "MessagesFiles";

    public Guid Id { get; set; }
    public Guid MessageId { get; set; }
    public Guid FileId { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public Guid? ModifiedBy { get; set; }
    public DateTime? ModifiedAtUtc { get; set; }

    public DbMessage Message { get; set; }
    public DbThreadMessage ThreadMessage { get; set; }
  }

  public class DbMessageFileConfiguration : IEntityTypeConfiguration<DbMessageFile>
  {
    public void Configure(EntityTypeBuilder<DbMessageFile> builder)
    {
      builder
        .ToTable(DbMessageFile.TableName);

      builder
        .HasKey(mf => mf.Id);

      builder
        .HasOne(mf => mf.Message)
        .WithMany(m => m.Files);

      builder
        .HasOne(mf => mf.ThreadMessage)
        .WithMany(tm => tm.Files);
    }
  }
}
