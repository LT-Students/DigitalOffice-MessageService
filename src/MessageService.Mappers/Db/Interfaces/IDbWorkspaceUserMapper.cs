using System;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Db;

namespace LT.DigitalOffice.MessageService.Mappers.Db.Interfaces
{
  [AutoInject]
  public interface IDbWorkspaceUserMapper
  {
    DbWorkspaceUser Map(Guid userId, Guid workspaceId, bool isAdmin, Guid createdBy);
  }
}
