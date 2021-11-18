using System;
using System.Collections.Generic;
using LT.DigitalOffice.MessageService.Models.Dto.Models.Image;
using LT.DigitalOffice.MessageService.Models.Dto.Models.User;

namespace LT.DigitalOffice.MessageService.Models.Dto.Responses.Message
{
  public record MessageInfo
  {
    public Guid Id { get; set; }
    public string Content { get; set; }
    public int Status { get; set; }
    public int ThreadMessagesCount { get; set; }
    public UserInfo CreatedBy { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public List<Guid> FilesIds { get; set; }
    public List<ImageInfo> Images { get; set; }
  }
}
