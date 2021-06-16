using LT.DigitalOffice.MessageService.Models.Dto.Enums;
using LT.DigitalOffice.MessageService.Models.Dto.Models.Emails;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.MessageService.Models.Dto.Requests.EmailTemplate
{
    public class EmailTemplateRequest
    {
        public string Name { get; set; }
        public EmailTemplateType Type { get; set; }
        public Guid AuthorId { get; set; }
        public IEnumerable<EmailTemplateTextInfo> EmailTemplateTexts { get; set; }
    }
}
