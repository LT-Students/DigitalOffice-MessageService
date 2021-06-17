using System;

namespace LT.DigitalOffice.MessageService.Models.Dto.Models.Emails
{
    public class UnsentEmailInfo
    {
        public Guid Id { get; set; }
        public EmailInfo Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastSendAt { get; set; }
        public uint TotalSendingCount { get; set; }
    }
}
