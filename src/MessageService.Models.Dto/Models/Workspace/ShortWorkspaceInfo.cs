using System;

namespace LT.DigitalOffice.MessageService.Models.Dto.Models
{
  public record ShortWorkspaceInfo
  {
    public Guid Id { get; set; }
    public ImageInfo Image { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
  }
}
