﻿using System;
using System.Collections.Generic;
using LT.DigitalOffice.MessageService.Models.Dto.Models.Image;

namespace LT.DigitalOffice.MessageService.Models.Dto.Requests
{
  public record CreateChannelRequest
  {
    public string Name { get; set; }
    public Guid WorkspaceId { get; set; }
    public bool IsPrivate { get; set; }
    public ImageConsist Image { get; set; }
    public List<Guid> UsersIds { get; set; }
  }
}
