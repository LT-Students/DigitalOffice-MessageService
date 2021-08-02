using LT.DigitalOffice.MessageService.Models.Dto.Enums;
using System;

namespace LT.DigitalOffice.MessageService.Models.Dto.Models
{
    public record KeywordInfo
    {
        public Guid Id { get; set; }
        public string Keyword { get; set; }
        public ServiceName ServiceName { get; set; }
        public string EntityName { get; set; }
        public string PropertyName { get; set; }
    }
}
