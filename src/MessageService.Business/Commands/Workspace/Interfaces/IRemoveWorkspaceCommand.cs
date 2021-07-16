﻿using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using System;

namespace LT.DigitalOffice.MessageService.Business.Commands.Workspace.Interfaces
{
    [AutoInject]
    public interface IRemoveWorkspaceCommand
    {
        OperationResultResponse<bool> Execute(Guid workspaceId);
    }
}
