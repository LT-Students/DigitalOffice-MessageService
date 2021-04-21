using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace;
using System;

namespace LT.DigitalOffice.MessageService.Mappers.WorkspaceMappers.Interfaces
{
    [AutoInject]
    public interface IDbWorkspaceMapper
    {
        DbWorkspace Map(WorkspaceRequest value, Guid ownerId, Guid? imageId);
    }
}
