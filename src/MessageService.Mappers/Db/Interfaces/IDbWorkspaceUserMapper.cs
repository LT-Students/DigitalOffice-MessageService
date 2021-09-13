using System;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Db;

namespace LT.DigitalOffice.MessageService.Mappers.Db.Interfaces
{
  [AutoInject]
  public interface IDbWorkspaceUserMapper
  {
    DbWorkspaceUser Map(Guid workspaceId, Guid userId, bool isAdmin, Guid createdBy);
  }
}
