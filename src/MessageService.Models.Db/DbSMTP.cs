using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace LT.DigitalOffice.MessageService.Models.Db
{
    public class DbSMTP
    {
        public const string TableName = "SMTP";

        public Guid Id { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class DbSMTPConfiguration : IEntityTypeConfiguration<DbSMTP>
    {
        public void Configure(EntityTypeBuilder<DbSMTP> builder)
        {
            builder
                .ToTable(DbSMTP.TableName);

            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Host)
                .IsRequired();

            builder
                .Property(x => x.Email)
                .IsRequired();

            builder
                .Property(x => x.Password)
                .IsRequired();
        }
    }
}
