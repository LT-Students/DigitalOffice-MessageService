﻿using LT.DigitalOffice.Kernel.Exceptions;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto;
using System;
using LT.DigitalOffice.MessageService.Mappers.WorkspaceMappers.Interfaces;

namespace LT.DigitalOffice.MessageService.Mappers.WorkspaceMappers
{
    public class DbWorkspaceMapper : IDbWorkspaceMapper
    {
        public DbWorkspace Map(Workspace value, Guid ownerId, Guid? imageId)
        {
            if (value == null)
            {
                throw new BadRequestException("Value was null");
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