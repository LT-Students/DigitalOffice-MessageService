using System;

namespace LT.DigitalOffice.MessageService.Models.Dto.Requests.Channel
{
  public record CreateChannelUserRequest
  {
    public Guid WorkspaceUserId { get; set; }
    public bool IsAdmin { get; set; }
  }
}
