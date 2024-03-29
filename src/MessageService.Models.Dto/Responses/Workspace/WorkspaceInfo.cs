﻿using System;
using System.Collections.Generic;
using LT.DigitalOffice.MessageService.Models.Dto.Models.Image;
using LT.DigitalOffice.MessageService.Models.Dto.Models.User;
using LT.DigitalOffice.MessageService.Models.Dto.Responses.Channel;

namespace LT.DigitalOffice.MessageService.Models.Dto.Responses.Workspace
{
  public record WorkspaceInfo
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public ImageConsist Image { get; set; }
    public UserInfo CreatedBy { get; set; }
    public DateTime CreatedAtUtc { get; set; }

    public List<ShortChannelInfo> Channels { get; set; }
    public List<UserInfo> Users { get; set; }
  }
}
