using LT.DigitalOffice.MessageService.Models.Dto.Models;

namespace LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace
{
    public record WorkspaceRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ImageInfo Image { get; set; }
    }
}
