using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace LT.DigitalOffice.MessageService.Models.Db
{
    public class DbEmail
    {
        public const string TableName = "Emails";

        public Guid Id { get; set; }
        public Guid? SenderId { get; set; }
        public string Receiver { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public DbUnsentEmail UnsentEmail { get; set; }
    }

    public class DbEmailConfiguration : IEntityTypeConfiguration<DbEmail>
    {
        public void Configure(EntityTypeBuilder<DbEmail> builder)
        {
            builder
                .ToTable(DbEmail.TableName);

            builder
                .HasKey(e => e.Id);

            builder
                .Property(e => e.Receiver)
                .IsRequired();

            builder
                .Property(e => e.Subject)
                .IsRequired();

            builder
                .Property(e => e.Body)
                .IsRequired();

            builder
                .HasOne(e => e.UnsentEmail)
                .WithOne(ue => ue.Email);
        }
    }
}