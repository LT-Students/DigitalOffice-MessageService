using System;

namespace LT.DigitalOffice.MessageService.Models.Dto
{
    public class Message
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public Guid SenderUserId { get; set; }
        public Guid RecipientUserId { get; set; }
    }
}