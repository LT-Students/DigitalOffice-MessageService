using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests;
using System;

namespace LT.DigitalOffice.MessageService.Mappers.WorkspaceMappers.Interfaces
{
    [AutoInject]
    public interface IDbWorkspaceMapper
    {
        DbWorkspace Map(Workspace value, Guid ownerId, Guid? imageId);
    }
}
