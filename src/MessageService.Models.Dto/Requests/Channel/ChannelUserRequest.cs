using System;

namespace LT.DigitalOffice.MessageService.Models.Dto.Requests.Channel
{
  public record ChannelUserRequest
  {
    public Guid WorkspaceUserId { get; set; }
    public bool IsAdmin { get; set; }
  }
}
