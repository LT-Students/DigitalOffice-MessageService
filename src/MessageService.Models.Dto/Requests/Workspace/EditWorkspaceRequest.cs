using LT.DigitalOffice.MessageService.Models.Dto.Models.Image;

namespace LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace
{
  public record EditWorkspaceRequest
  {
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public ImageContent Image { get; set; }
  }
}
