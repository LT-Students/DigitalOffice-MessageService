using LT.DigitalOffice.MessageService.Mappers.WorkspaceMappers.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace;
using LT.DigitalOffice.Models.Broker.Requests.Message;
using System;
using System.Linq;

namespace LT.DigitalOffice.MessageService.Mappers.Db
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
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
        }

        public DbWorkspace Map(ICreateWorkspaceRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return new DbWorkspace
            {
                Id = Guid.NewGuid(),
                OwnerId = request.CreaterId,
                Name = request.Name,
                Description = "", // TODO Create description for default workspace
                ImageId = null,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
        }
    }
}
