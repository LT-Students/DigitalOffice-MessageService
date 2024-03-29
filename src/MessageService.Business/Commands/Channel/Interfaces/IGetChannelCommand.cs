﻿using System;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Models.Dto.Filtres;
using LT.DigitalOffice.MessageService.Models.Dto.Responses.Channel;

namespace LT.DigitalOffice.MessageService.Business.Commands.Channel.Interfaces
{
  [AutoInject]
  public interface IGetChannelCommand
  {
    Task<OperationResultResponse<ChannelInfo>> ExeсuteAsync(
      Guid channelId, GetChannelFilter filter);
  }
}
