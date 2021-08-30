using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LT.DigitalOffice.MessageService.Models.Db
{
  public class DbEmailTemplateText
  {
    public const string TableName = "EmailTemplateTexts";

    public Guid Id { get; set; }
    public Guid EmailTemplateId { get; set; }
    public string Subject { get; set; }
    public string Text { get; set; }
    public string Language { get; set; }

    public DbEmailTemplate EmailTemplate { get; set; }

  }

  public class DbEmailTemplateTextConfiguration : IEntityTypeConfiguration<DbEmailTemplateText>
  {
    public void Configure(EntityTypeBuilder<DbEmailTemplateText> builder)
    {
      builder
          .ToTable(DbEmailTemplateText.TableName);

      builder
          .HasKey(ett => ett.Id);

      builder
          .Property(ett => ett.Subject)
          .IsRequired();

      builder
          .Property(ett => ett.Language)
          .HasMaxLength(2)
          .IsRequired();

      builder
          .Property(ett => ett.Text)
          .IsRequired();

      builder
          .HasOne(ett => ett.EmailTemplate)
          .WithMany(et => et.EmailTemplateTexts);
    }
  }
}
