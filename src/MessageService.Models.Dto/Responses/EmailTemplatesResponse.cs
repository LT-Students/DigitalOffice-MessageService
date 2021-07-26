using LT.DigitalOffice.MessageService.Models.Dto.Models.Emails;
using System.Collections.Generic;

namespace LT.DigitalOffice.MessageService.Models.Dto.Responses
{
    public record EmailTemplatesResponse
    {
        public int TotalCount { get; set; }
        public List<EmailTemplateInfo> Emails { get; set; } = new();
    }
}
