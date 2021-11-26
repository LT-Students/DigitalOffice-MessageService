using System;
using System.Collections.Generic;
using LT.DigitalOffice.MessageService.Models.Dto.Models.Image;
using LT.DigitalOffice.MessageService.Models.Dto.Models.User;
using LT.DigitalOffice.MessageService.Models.Dto.Responses.Message;

namespace LT.DigitalOffice.MessageService.Models.Dto.Responses.Channel
{
  public record ChannelInfo
  {
    public Guid Id { get; set; }
    public ImageConsist Avatar { get; set; }
    public string Name { get; set; }
    public bool IsPrivate { get; set; }

    public List<MessageInfo> Messages { get; set; }
    public List<UserInfo> Users { get; set; }
  }
}
