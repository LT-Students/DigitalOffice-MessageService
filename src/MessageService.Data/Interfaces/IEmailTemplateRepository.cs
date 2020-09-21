using System;

namespace LT.DigitalOffice.MessageService.Data.Interfaces
{
    /// <summary>
    /// Represents interface of repository in repository pattern.
    /// Provides methods for working with EmailTemplate in the database of MessageService.
    /// </summary>
    public interface IEmailTemplateRepository
    {
        /// <summary>
        /// Disable an email template by its Id.
        /// </summary>
        /// <param name="emailTemplateId">Email template Id.</param>
        void RemoveEmailTemplate(Guid emailTemplateId);
    }
}
