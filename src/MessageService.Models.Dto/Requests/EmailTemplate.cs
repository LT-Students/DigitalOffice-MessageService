using System;

namespace LT.DigitalOffice.MessageService.Models.Dto.Requests
{
    public class EmailTemplate
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public Guid AuthorId { get; set; }
    }
}
