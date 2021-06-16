using LT.DigitalOffice.MessageService.Models.Dto.Models.Emails;
using System.Collections.Generic;

namespace LT.DigitalOffice.MessageService.Models.Dto.Responses
{
    public class UnsentEmailsResponse
    {
        public int TotalCount { get; set; }
        public List<UnsentEmailInfo> Emails { get; set; } = new();
    }
}
