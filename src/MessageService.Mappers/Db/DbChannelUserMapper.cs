﻿using System;
using LT.DigitalOffice.MessageService.Mappers.Db.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;

namespace LT.DigitalOffice.MessageService.Mappers.Db
{
  public class DbChannelUserMapper : IDbChannelUserMapper
  {
    public DbChannelUser Map(Guid channelId, Guid UserId, bool isAdmin, Guid createdBy)
    {
      return new DbChannelUser()
      {
        Id = Guid.NewGuid(),
        UserId = UserId,
        ChannelId = channelId,
        IsAdmin = isAdmin,
        IsActive = true,
        CreatedBy = createdBy,
        CreatedAtUtc = DateTime.UtcNow
      };
    }
  }
}
