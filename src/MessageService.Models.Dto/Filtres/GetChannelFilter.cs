using System;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.MessageService.Models.Dto.Filtres
{
  public class GetChannelFilter
  {
    [FromQuery(Name = "channelid")]
    public Guid ChannelId { get; set; }

    [FromQuery(Name = "skipmessages")]
    public int SkipMessages { get; set; } = 0;

    [FromQuery(Name = "takemessages")]
    public int TakeMessages { get; set; }
  }
}
