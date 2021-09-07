using System;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests;

namespace LT.DigitalOffice.MessageService.Mappers.Db.Interfaces
{
  [AutoInject]
  public interface IDbChannelMapper
  {
    DbChannel Map(CreateChannelRequest request, Guid CreatedBy);
    DbChannel Map(Guid workspaceId, Guid createdBy);
  }
}
