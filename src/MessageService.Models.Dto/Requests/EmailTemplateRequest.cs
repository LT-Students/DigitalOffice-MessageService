using LT.DigitalOffice.MessageService.Models.Dto.Enums;
using LT.DigitalOffice.MessageService.Models.Dto.Models;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.MessageService.Models.Dto
{
    public class EmailTemplateRequest
    {
        public string Name { get; set; }
        public EmailTemplateType Type { get; set; }
        public Guid AuthorId { get; set; }
        public IEnumerable<EmailTemplateTextInfo> EmailTemplateTexts { get; set; }
    }
}
