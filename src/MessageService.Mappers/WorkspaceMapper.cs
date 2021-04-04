using LT.DigitalOffice.Kernel.Exceptions;
using LT.DigitalOffice.MessageService.Mappers.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto;
using System;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;

namespace LT.DigitalOffice.MessageService.Mappers
{
    public class WorkspaceMapper : IMapper<AddWorkspaceRequest, DbWorkspace>
    {
        public DbWorkspace Map(AddWorkspaceRequest value)
        {
            if (value == null)
            {
                throw new BadRequestException();
            }

            return new DbWorkspace
            {
                Id = Guid.NewGuid(),
                Name = value.Name,
                Description = value.Description,
                Image = value.Image,
                IsActive = true
            };
        }
    }
}
