using LT.DigitalOffice.Models.Broker.Enums;
using System.Collections.Generic;

namespace LT.DigitalOffice.MessageService.Models.Dto.Requests.EmailTemplate
{
    public record EmailTemplateRequest
    {
        public string Name { get; set; }
        public EmailTemplateType Type { get; set; }
        public IEnumerable<EmailTemplateTextRequest> EmailTemplateTexts { get; set; }
    }
}
