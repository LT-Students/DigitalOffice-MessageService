using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace LT.DigitalOffice.MessageService.Models.Db
{
    public class DbEmailReciever
    {
        public Guid EmailId { get; set; }
        public DbEmail Email { get; set; }
        public string RecieverEmail { get; set; }
    }

    public class EmailRecieversConfiguration : IEntityTypeConfiguration<DbEmailReciever>
    {
        public void Configure(EntityTypeBuilder<DbEmailReciever> builder)
        {
            builder.HasKey(emailReciever => new { emailReciever.EmailId, emailReciever.RecieverEmail });
        }
    }
}