using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests;

namespace LT.DigitalOffice.MessageService.Mappers.Db.Interfaces
{
  [AutoInject]
  public interface IDbChannelMapper
  {
    Task<DbChannel> MapAsync(CreateChannelRequest request);

    DbChannel Map(Guid workspaceId, List<DbWorkspaceUser> workspaseUsers, Guid createdBy);
  }
}
