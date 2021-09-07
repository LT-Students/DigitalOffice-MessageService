using System;
using System.Collections.Generic;
using LT.DigitalOffice.MessageService.Models.Dto.Models.Workspace;

namespace LT.DigitalOffice.MessageService.Models.Dto.Requests
{
  public record CreateChannelRequest
  {
    public string Name { get; set; }
    public string Pinned { get; set; }
    public Guid WorkspaceId { get; set; }
    public bool IsPrivate { get; set; }
    public Image Image { get; set; }
    public List<Guid> Users { get; set; }
  }
}
