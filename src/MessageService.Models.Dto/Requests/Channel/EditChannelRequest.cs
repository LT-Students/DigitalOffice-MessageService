namespace LT.DigitalOffice.MessageService.Models.Dto.Requests.Channel
{
  public record EditChannelRequest
  {
    public string Name { get; set; }
    public bool IsPrivate { get; set; }
    public bool IsActive { get; set; }
    public string ImageContent { get; set; }
    public string ImageExtension { get; set; }
  }
}
