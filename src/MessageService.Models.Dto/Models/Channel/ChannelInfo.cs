using System;
using System.Collections.Generic;
using LT.DigitalOffice.MessageService.Models.Dto.Models.User;
using LT.DigitalOffice.MessageService.Models.Dto.Models.Workspace;

namespace LT.DigitalOffice.MessageService.Models.Dto.Models.Channel
{
  public record ChannelInfo
  {
    public Guid Id { get; set; }
    public ShortWorkspaceInfo WorkspaceId { get; set; }
    public Image Avatar { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public bool IsPrivate { get; set; }

    public List<UserInfo> Users { get; set; }
  }
}
