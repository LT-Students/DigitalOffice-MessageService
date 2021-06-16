﻿using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using System;

namespace LT.DigitalOffice.MessageService.Business.EmailTemplatesCommands.Interfaces
{
    /// <summary>
    /// Represents interface for a command in command pattern.
    /// Provides method for adding an email template.
    /// </summary>
    [AutoInject]
    public interface IDisableEmailTemplateCommand
    {
        /// <summary>
        ///  Disable an email template.
        /// </summary>
        /// <param name="emailTemplateId">Email template Id.</param>
        OperationResultResponse<bool> Execute(Guid emailTemplateId);
    }
}