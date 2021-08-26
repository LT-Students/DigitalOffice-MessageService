using System;

namespace LT.DigitalOffice.MessageService.Models.Dto.Models.Channel
{
    public record ShortChannelInfo
    {
        public Guid Id { get; set; }
        public ImageInfo Image { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsPrivate { get; set; }
    }
}
