using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Db;
using System;

namespace LT.DigitalOffice.MessageService.Data.Interfaces
{
    /// <summary>
    /// Represents interface of repository in repository pattern.
    /// Provides methods for working with EmailTemplate in the database of MessageService.
    /// </summary>
    [AutoInject]
    public interface IEmailTemplateRepository
    {
        /// <summary>
        /// Disable an email template by its Id.
        /// </summary>
        /// <param name="emailTemplateId">Email template Id.</param>
        bool DisableEmailTemplate(Guid emailTemplateId);

        /// <summary>
        /// Adds new email template to the database.
        /// </summary>
        /// <param name="emailTemplate">Email template to add.</param>
        /// <returns>Guid of added email template.</returns>
        Guid AddEmailTemplate(DbEmailTemplate emailTemplate);

        /// <summary>
        /// Edit email template to the database.
        /// </summary>
        /// <param name="dbEmailTemplateToEdit">Email template data to update.</param>
        bool EditEmailTemplate(DbEmailTemplate dbEmailTemplateToEdit);

        /// <summary>
        /// Get email template by id from the database.
        /// </summary>
        /// <param name="id">Email template id.</param>
        DbEmailTemplate GetEmailTemplateById(Guid id);

        /// <summary>
        /// Get first email template by type from the database.
        /// </summary>
        /// <param name="type">Email template type.</param>
        DbEmailTemplate GetEmailTemplateByType(int type);
    }
}
