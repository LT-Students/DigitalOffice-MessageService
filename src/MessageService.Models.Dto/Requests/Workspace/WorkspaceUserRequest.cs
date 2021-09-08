using System;

namespace LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace
{
  public record WorkspaceUserRequest
  {
    public Guid UserId { get; set; }
    public bool IsAdmin { get; set; }
  }
}
