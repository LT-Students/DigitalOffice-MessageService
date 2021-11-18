using System;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.MessageService.Models.Dto.Filtres
{
  public class GetChannelFilter
  {
    [FromQuery(Name = "channelid")]
    public Guid ChannelId { get; set; }

    [FromQuery(Name = "skipmessagescount")]
    public int SkipMessagesCount { get; set; } = 0;

    [FromQuery(Name = "takemessagescount")]
    public int TakeMessagesCount { get; set; }
  }
}
