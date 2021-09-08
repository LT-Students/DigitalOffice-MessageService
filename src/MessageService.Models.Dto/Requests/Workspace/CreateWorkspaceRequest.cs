using System.Collections.Generic;
using LT.DigitalOffice.MessageService.Models.Dto.Models.Image;

namespace LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace
{
  public record CreateWorkspaceRequest
  {
    public string Name { get; set; }
    public string Description { get; set; }
    public ImageContent Image { get; set; }
    public List<WorkspaceUserRequest> Users { get; set; }
  }
}
