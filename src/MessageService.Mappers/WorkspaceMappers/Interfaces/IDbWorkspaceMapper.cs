﻿using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto;
using System;

namespace LT.DigitalOffice.MessageService.Mappers.WorkspaceMappers.Interfaces
{
    public interface IDbWorkspaceMapper
    {
        DbWorkspace Map(Workspace value, Guid ownerId, Guid? imageId);
    }
}
