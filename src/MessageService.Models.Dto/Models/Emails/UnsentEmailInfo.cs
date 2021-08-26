using System;

namespace LT.DigitalOffice.MessageService.Models.Dto.Models.Emails
{
    public record UnsentEmailInfo
    {
        public Guid Id { get; set; }
        public EmailInfo Email { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime LastSendAtUtc { get; set; }
        public uint TotalSendingCount { get; set; }
    }
}
