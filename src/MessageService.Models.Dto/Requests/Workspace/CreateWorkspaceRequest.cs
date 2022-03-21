using System;
using System.Collections.Generic;
using LT.DigitalOffice.MessageService.Models.Dto.Models.Image;

namespace LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace
{
  public record CreateWorkspaceRequest
  {
    public string Name { get; set; }
    public string Description { get; set; }
    public ImageConsist Image { get; set; }
    public List<Guid> Users { get; set; }
  }
}
