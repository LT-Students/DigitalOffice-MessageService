using System;

namespace LT.DigitalOffice.MessageService.Models.Dto
{
    public class EditEmailTemplateRequest
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
