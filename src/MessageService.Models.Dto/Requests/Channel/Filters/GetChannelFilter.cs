using System;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.MessageService.Models.Dto.Requests.Channel.Filters
{
  public record GetChannelFilter
  {
    [FromQuery(Name = "channelId")]
    public Guid ChannelId { get; set; }

    [FromQuery(Name = "includeUsers")]
    public bool IncludeUsers { get; set; } = false;
  }
}
