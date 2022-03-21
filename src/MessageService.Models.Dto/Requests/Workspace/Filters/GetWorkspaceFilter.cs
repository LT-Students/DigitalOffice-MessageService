using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace.Filters
{
  public record GetWorkspaceFilter
  {
    [FromQuery(Name = "includeUsers")]
    public bool IncludeUsers { get; set; } = false;

    [FromQuery(Name = "includeChannels")]
    public bool IncludeChannels { get; set; } = false;
  }
}
