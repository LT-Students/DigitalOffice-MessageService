using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace LT.DigitalOffice.MessageService.Models.Db
{
    public class DbMessageFile
    {
        public Guid MessageId { get; set; }
        public DbMessage Message { get; set; }
        public Guid FileId { get; set; }
    }

    public class MessageFileConfiguration : IEntityTypeConfiguration<DbMessageFile>
    {
        public void Configure(EntityTypeBuilder<DbMessageFile> builder)
        {
            builder.HasKey(messageFile => new { messageFile.MessageId, messageFile.FileId });
        }
    }
}