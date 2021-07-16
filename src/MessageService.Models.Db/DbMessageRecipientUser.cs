using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace LT.DigitalOffice.MessageService.Models.Db
{
    public class DbMessageRecipientUser
    {
        public Guid MessageId { get; set; }
        public DbMessage Message { get; set; }
        public Guid RecipientUserId { get; set; }
    }

    public class ProjectWorkerUserConfiguration : IEntityTypeConfiguration<DbMessageRecipientUser>
    {
        public void Configure(EntityTypeBuilder<DbMessageRecipientUser> builder)
        {
            builder.HasKey(mr => new { mr.MessageId, mr.RecipientUserId });

            builder.HasOne(mr => mr.Message)
                .WithMany(m => m.RecipientUsersIds)
                .HasForeignKey(mr => mr.MessageId);
        }
    }
}