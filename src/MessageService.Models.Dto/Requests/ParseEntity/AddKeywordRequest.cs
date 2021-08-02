using LT.DigitalOffice.MessageService.Models.Dto.Enums;

namespace LT.DigitalOffice.MessageService.Models.Dto.Requests.ParseEntity
{
    public record AddKeywordRequest
    {
        public string Keyword { get; set; }
        public ServiceName ServiceName { get; set; }
        public string EntityName { get; set; }
        public string PropertyName { get; set; }
    }
}
