using System;
using LT.DigitalOffice.MessageService.Models.Dto.Enums;

namespace LT.DigitalOffice.MessageService.Models.Dto.Requests.Message
{
  public record CreateMessageRequest
  {
    public Guid ChannelId { get; set; }
    public string Content { get; set; }
    public StatusType Status { get; set; }
  }
}
