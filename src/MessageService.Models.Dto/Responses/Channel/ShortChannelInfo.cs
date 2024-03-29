﻿using System;
using LT.DigitalOffice.MessageService.Models.Dto.Models.Image;

namespace LT.DigitalOffice.MessageService.Models.Dto.Responses.Channel
{
  public record ShortChannelInfo
  {
    public Guid Id { get; set; }
    public ImageConsist Avatar { get; set; }
    public string Name { get; set; }
    public bool IsPrivate { get; set; }
  }
}
