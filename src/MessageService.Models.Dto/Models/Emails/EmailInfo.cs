using System;

namespace LT.DigitalOffice.MessageService.Models.Dto.Models.Emails
{
    public record EmailInfo
    {
        public Guid Id { get; set; }
        public string Receiver { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
