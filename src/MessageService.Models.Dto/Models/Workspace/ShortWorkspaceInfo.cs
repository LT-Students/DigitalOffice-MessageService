using System;
using LT.DigitalOffice.MessageService.Models.Dto.Models.Workspace;

namespace LT.DigitalOffice.MessageService.Models.Dto.Models
{
  public record ShortWorkspaceInfo
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public AvatarData Avatar { get; set; }
  }
}
