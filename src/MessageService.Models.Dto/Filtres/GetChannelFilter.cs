﻿using System;
using LT.DigitalOffice.Kernel.Requests;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.MessageService.Models.Dto.Filtres
{
  public record GetChannelFilter : BaseFindFilter
  {
    [FromQuery(Name = "channelid")]
    public Guid ChannelId { get; set; }
  }
}