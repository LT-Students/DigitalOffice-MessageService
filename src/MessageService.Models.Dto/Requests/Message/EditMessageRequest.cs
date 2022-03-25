using System;
using LT.DigitalOffice.MessageService.Models.Dto.Enums;

namespace LT.DigitalOffice.MessageService.Models.Dto.Requests.Message
{
  public class EditMessageRequest
  {
    public string Content { get; set; }
    public StatusType Status { get; set; }
  }
}
