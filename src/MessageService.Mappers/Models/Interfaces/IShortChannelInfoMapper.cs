﻿using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Responses.Channel;

namespace LT.DigitalOffice.MessageService.Mappers.Models.Interfaces
{
  [AutoInject]
  public interface IShortChannelInfoMapper
  {
    ShortChannelInfo Map(DbChannel dbChannel);
  }
}
