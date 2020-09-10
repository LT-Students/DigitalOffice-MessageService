using LT.DigitalOffice.MessageService.Models.Dto.Enums;

namespace LT.DigitalOffice.MessageService.Models.Dto
{
    public class Message
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public StatusType Status { get; set; }
    }
}
