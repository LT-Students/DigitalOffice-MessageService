using LT.DigitalOffice.MessageService.Models.Dto.Enums;
using LT.DigitalOffice.MessageService.Models.Dto.Models;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.MessageService.Models.Dto.Requests
{
    public class EditEmailTemplateRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public EmailTemplateType Type { get; set; }
        public IEnumerable<EmailTemplateTextInfo> EmailTemplateTexts { get; set; }
    }
}
