using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LT.DigitalOffice.MessageService.Models.Db
{
  public class DbMessageImage
  {
    public const string TableName = "MessagesImages";

    public Guid Id { get; set; }
    public Guid MessageId { get; set; }
    public Guid ImageId { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public Guid? ModifiedBy { get; set; }
    public DateTime? ModifiedAtUtc { get; set; }

    public DbMessage Message { get; set; }
    public DbThreadMessage ThreadMessage { get; set; }
  }

  public class DbMessageImageConfiguration : IEntityTypeConfiguration<DbMessageImage>
  {
    public void Configure(EntityTypeBuilder<DbMessageImage> builder)
    {
      builder
        .ToTable(DbMessageImage.TableName);

      builder
        .HasKey(mi => mi.Id);

      builder
        .HasOne(mi => mi.Message)
        .WithMany(m => m.Images);

      builder
        .HasOne(mi => mi.ThreadMessage)
        .WithMany(tm => tm.Images);
    }
  }
}
