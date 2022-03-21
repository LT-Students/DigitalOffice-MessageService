namespace LT.DigitalOffice.MessageService.Models.Dto.Requests.WorkspaceUser
{
  public record EditWorkspaceUserRequest
  {
    public bool IsAdmin { get; set; }
    public bool IsActive { get; set; }
  }
}
