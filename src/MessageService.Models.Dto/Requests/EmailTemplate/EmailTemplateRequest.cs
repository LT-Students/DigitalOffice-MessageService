using LT.DigitalOffice.MessageService.Models.Dto.Models.Emails;
using LT.DigitalOffice.Models.Broker.Enums;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.MessageService.Models.Dto.Requests.EmailTemplate
{
    public record EmailTemplateRequest
    {
        public string Name { get; set; }
        public EmailTemplateType Type { get; set; }
        public Guid AuthorId { get; set; }
        public IEnumerable<EmailTemplateTextInfo> EmailTemplateTexts { get; set; }
    }
}
