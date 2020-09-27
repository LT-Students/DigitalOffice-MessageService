using LT.DigitalOffice.MessageService.Models.Dto;
using System;

namespace LT.DigitalOffice.MessageService.Business.Interfaces
{
    /// <summary>
    /// Represents interface for a command in command pattern.
    /// Provides method for adding an email template.
    /// </summary>
    public interface IAddEmailTemplateCommand
    {
        /// <summary>
        ///  Adding a new email template.
        /// </summary>
        /// <param name="emailTemplate">Email template data.</param>
        /// <returns>Guid of the added email template.</returns>
        Guid Execute(EmailTemplate emailTemplate, Guid requestingUserId);
    }
}
