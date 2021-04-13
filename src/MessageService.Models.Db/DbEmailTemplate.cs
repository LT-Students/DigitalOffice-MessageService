using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.MessageService.Models.Db
{
    public class DbEmailTemplate
    {
        public const string TableName = "EmailTemplates";

        public Guid Id { get; set; }
        public Guid AuthorId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Type { get; set; }
        public bool IsActive { get; set; }

        public ICollection<DbEmailTemplateText> EmailTemplateTexts { get; set; }

        public DbEmailTemplate()
        {
            EmailTemplateTexts = new HashSet<DbEmailTemplateText>();
        }
    }

    public class DbEmailTemplateConfiguration : IEntityTypeConfiguration<DbEmailTemplate>
    {
        public void Configure(EntityTypeBuilder<DbEmailTemplate> builder)
        {
            builder
                .ToTable(DbEmailTemplate.TableName);

            builder
                .HasKey(et => et.Id);

            builder
                .Property(et => et.Name)
                .IsRequired();

            builder
                .HasMany(et => et.EmailTemplateTexts)
                .WithOne(et => et.EmailTemplate);
        }
    }
}
