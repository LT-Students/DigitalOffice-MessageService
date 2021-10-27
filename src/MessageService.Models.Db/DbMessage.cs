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
    public string Content { get; set; }
    public int Status { get; set; }
    public Guid SenderUserId { get; set; }
    public DateTime CreatedAtUtc { get; set; }

    public ICollection<DbMessageRecipientUser> RecipientUsersIds { get; set; }
    public ICollection<DbMessageFile> FilesIds { get; set; }
  }

  public class DbWMessageConfiguration : IEntityTypeConfiguration<DbMessage>
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
          .HasMany(w => w.RecipientUsersIds)
          .WithOne(wa => wa.Message);

      builder
          .HasMany(w => w.FilesIds)
          .WithOne(ch => ch.Message);
    }
  }
}
