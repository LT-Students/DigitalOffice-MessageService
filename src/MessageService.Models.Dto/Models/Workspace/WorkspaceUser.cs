using System;
using LT.DigitalOffice.MessageService.Models.Dto.Models.User;

namespace LT.DigitalOffice.MessageService.Models.Dto.Models.Workspace
{
  public record WorkspaceUser
  {
    public Guid Id { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsActive { get; set; }
    public UserInfo User { get; set; }
  }
}
