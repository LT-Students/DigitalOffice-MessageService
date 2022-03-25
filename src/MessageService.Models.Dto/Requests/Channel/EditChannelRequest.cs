using LT.DigitalOffice.MessageService.Models.Dto.Models.Image;

namespace LT.DigitalOffice.MessageService.Models.Dto.Requests.Channel
{
  public record EditChannelRequest
  {
    public string Name { get; set; }
    public bool IsPrivate { get; set; }
    public bool IsActive { get; set; }
    public ImageConsist Image { get; set; }
  }
}
