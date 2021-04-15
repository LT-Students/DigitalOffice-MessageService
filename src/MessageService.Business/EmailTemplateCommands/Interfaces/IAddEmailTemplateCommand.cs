using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Dto.Requests;
using System;

namespace LT.DigitalOffice.MessageService.Business.EmailTemplatesCommands.Interfaces
{
    /// <summary>
    /// Represents interface for a command in command pattern.
    /// Provides method for adding an email template.
    /// </summary>
    [AutoInject]
    public interface IAddEmailTemplateCommand
    {
        /// <summary>
        ///  Adding a new email template.
        /// </summary>
        /// <param name="emailTemplate">Email template data.</param>
        /// <returns>Guid of the added email template.</returns>
        Guid Execute(EmailTemplateRequest emailTemplate);
    }
}
