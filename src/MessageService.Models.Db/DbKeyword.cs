using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace LT.DigitalOffice.MessageService.Models.Db
{
    public class DbKeyword
    {
        public const string TableName = "Keywords";

        public Guid Id { get; set; }
        public string Keyword { get; set; }
        public int ServiceName { get; set; }
        public string EntityName { get; set; }
        public string PropertyName { get; set; }
    }

    public class DbParseEntityConfiguration : IEntityTypeConfiguration<DbKeyword>
    {
        public void Configure(EntityTypeBuilder<DbKeyword> builder)
        {
            builder
                .ToTable(DbKeyword.TableName);

            builder
                .HasKey(pe => pe.Id);

            builder
                .Property(pe => pe.Keyword)
                .IsRequired();

            builder
                .Property(pe => pe.EntityName)
                .IsRequired();

            builder
                .Property(pe => pe.PropertyName)
                .IsRequired();
        }
    }
}
