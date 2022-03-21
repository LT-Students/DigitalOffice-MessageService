namespace LT.DigitalOffice.MessageService.Models.Dto.Requests.ChannelUser
{
  public record EditChannelUserRequest
  {
    public bool IsAdmin { get; set; }
    public bool IsActive { get; set; }
  }
}
