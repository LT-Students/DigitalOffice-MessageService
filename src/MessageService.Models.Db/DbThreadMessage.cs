using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LT.DigitalOffice.MessageService.Models.Db
{
  public class DbThreadMessage
  {
    public const string TableName = "ThreadsMessages";

    public Guid Id { get; set; }
    public Guid MessageId { get; set; }
    public string Content { get; set; }
    public int Status { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public Guid? ModifiedBy { get; set; }
    public DateTime? ModifiedAtUtc { get; set; }

    public DbMessage Message { get; set; }
  }

  public class DbThreadMessageConfiguration : IEntityTypeConfiguration<DbThreadMessage>
  {
    public void Configure(EntityTypeBuilder<DbThreadMessage> builder)
    {
      builder
        .ToTable(DbThreadMessage.TableName);

      builder
        .HasKey(mi => mi.Id);

      builder
        .HasOne(tm => tm.Message)
        .WithMany(m => m.ThreadMessages);
    }
  }
}
