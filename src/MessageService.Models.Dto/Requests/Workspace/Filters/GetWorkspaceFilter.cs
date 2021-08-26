using System;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace.Filters
{
  public record GetWorkspaceFilter
  {
    [FromQuery(Name = "workspaceId")]
    public Guid WorkspaceId { get; set; }

    [FromQuery(Name = "includeUsers")]
    public bool? IncludeUsers { get; set; }

    [FromQuery(Name = "includeChannels")]
    public bool? IncludeChannels { get; set; }

    public bool IsIncludeUsers => IncludeUsers.HasValue && IncludeUsers.Value;
    public bool IsIncludeChannels => IncludeChannels.HasValue && IncludeChannels.Value;
  }
}
