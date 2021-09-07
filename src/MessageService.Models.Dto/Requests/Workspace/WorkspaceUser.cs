using System;

namespace LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace
{
  public record WorkspaceUser
  {
    public Guid UserId { get; set; }
    public bool IsAdmin { get; set; }
  }
}
