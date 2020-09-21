using System;

namespace LT.DigitalOffice.MessageService.Business.Interfaces
{
    /// <summary>
    /// Represents interface for a command in command pattern.
    /// Provides method for adding an email template.
    /// </summary>
    public interface IRemoveEmailTemplateCommand
    {
        /// <summary>
        ///  Disable an email template.
        /// </summary>
        /// <param name="emailTemplateId">Email template Id.</param>
        void Execute(Guid emailTemplateId, Guid requestingUserId);
    }
}
