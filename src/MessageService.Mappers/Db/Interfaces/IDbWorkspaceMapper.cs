using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace;
using LT.DigitalOffice.Models.Broker.Requests.Message;
using System;

namespace LT.DigitalOffice.MessageService.Mappers.Db.Workspace.Interfaces
{
    [AutoInject]
    public interface IDbWorkspaceMapper
    {
        DbWorkspace Map(CreateWorkspaceRequest value, Guid ownerId, Guid? imageId);

        DbWorkspace Map(ICreateWorkspaceRequest request);
    }
}
