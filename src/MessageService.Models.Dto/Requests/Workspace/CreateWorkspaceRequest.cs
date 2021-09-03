using LT.DigitalOffice.MessageService.Models.Dto.Models.Workspace;

namespace LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace
{
  public record CreateWorkspaceRequest
  {
    public string Name { get; set; }
    public string Description { get; set; }
    public Image Image { get; set; }
  }
}
