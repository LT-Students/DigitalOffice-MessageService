using System;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.MessageService.Mappers.Db.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Message;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.MessageService.Mappers.Db
{
  public class DbMessageMapper : IDbMessageMapper
  {
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DbMessageMapper(IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor;
    }

    public DbMessage Map(CreateMessageRequest request)
    {
      if (request is null)
      {
        return null;
      }

      return new DbMessage
      {
        Id = Guid.NewGuid(),
        ChannelId = request.ChannelId,
        Content = request.Content,
        Status = (int)request.Status,
        CreatedBy = _httpContextAccessor.HttpContext.GetUserId(),
        CreatedAtUtc = DateTime.UtcNow
      };
    }
  }
}
