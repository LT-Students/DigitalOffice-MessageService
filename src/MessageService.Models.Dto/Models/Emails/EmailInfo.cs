using System;

namespace LT.DigitalOffice.MessageService.Models.Dto.Models.Emails
{
    public class EmailInfo
    {
        public Guid Id { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
