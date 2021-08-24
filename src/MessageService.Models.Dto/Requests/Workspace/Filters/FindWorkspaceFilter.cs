using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace.Filters
{
    public record FindWorkspaceFilter
    {
        [FromQuery(Name = "skipCount")]
        public int SkipCount { get; set; }

        [FromQuery(Name = "takeCount")]
        public int TakeCount { get; set; }

        [FromQuery(Name = "includeDeactivated")]
        public bool? IncludeDeactivated { get; set; }

        [FromQuery(Name = "includeChannels")]
        public bool? IncludeChannels { get; set; }

        public bool IsIncludeDeactivated => IncludeDeactivated.HasValue && IncludeDeactivated.Value;
        public bool IsIncludeChannels => IncludeChannels.HasValue && IncludeChannels.Value;
    }
}
