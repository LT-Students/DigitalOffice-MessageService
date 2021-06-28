using LT.DigitalOffice.MessageService.Mappers.Db.Workspace.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace;
using System;

namespace LT.DigitalOffice.MessageService.Mappers.Db.Workspace
{
    public class DbWorkspaceMapper : IDbWorkspaceMapper
    {
        public DbWorkspace Map(WorkspaceRequest value, Guid ownerId, Guid? imageId)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return new DbWorkspace
            {
                Id = Guid.NewGuid(),
                OwnerId = ownerId,
                Name = value.Name,
                Description = value.Description,
                ImageId = imageId,
                IsActive = true
            };
        }
    }
}
