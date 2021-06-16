using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace LT.DigitalOffice.MessageService.Models.Db
{
    public class DbUnsentEmail
    {
        public const string TableName = "UnsentEmails";

        public Guid Id { get; set; }
        public Guid EmailId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastSendAt { get; set; }
        public uint TotalSendingCount { get; set; }
    }

    public class DbUnsentEmailConfiguration : IEntityTypeConfiguration<DbUnsentEmail>
    {
        public void Configure(EntityTypeBuilder<DbUnsentEmail> builder)
        {
            builder
                .ToTable(DbUnsentEmail.TableName);

            builder
                .HasKey(ue => ue.Id);
        }
    }
}
