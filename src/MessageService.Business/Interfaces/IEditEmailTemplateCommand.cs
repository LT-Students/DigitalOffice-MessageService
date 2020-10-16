using LT.DigitalOffice.MessageService.Models.Dto;
using System;

namespace LT.DigitalOffice.MessageService.Business.Interfaces
{
    /// <summary>
    /// Represents interface for a command in command pattern.
    /// Provides method for adding an email template.
    /// </summary>
    public interface IEditEmailTemplateCommand
    {
        /// <summary>
        /// Edit email template data with check right user.
        /// </summary>
        /// <param name="editEmailTemplate">Edit email template data.</param>
        void Execute(EditEmailTemplateRequest editEmailTemplate);
    }
}
