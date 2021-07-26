using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Db;
using System;
using System.Collections.Generic;

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
        bool Disable(Guid emailTemplateId);

        /// <summary>
        /// Adds new email template to the database.
        /// </summary>
        /// <param name="emailTemplate">Email template to add.</param>
        /// <returns>Guid of added email template.</returns>
        Guid Add(DbEmailTemplate emailTemplate);

        /// <summary>
        /// Edit email template to the database.
        /// </summary>
        /// <param name="dbEmailTemplateToEdit">Email template data to update.</param>
        bool Edit(DbEmailTemplate dbEmailTemplateToEdit);

        /// <summary>
        /// Get email template by id from the database.
        /// </summary>
        /// <param name="id">Email template id.</param>
        DbEmailTemplate Get(Guid id);

        /// <summary>
        /// Get first email template by type from the database.
        /// </summary>
        /// <param name="type">Email template type.</param>
        DbEmailTemplate Get(int type);

        List<DbEmailTemplate> Find(int skipCount, int takeCount, bool? includeDeactivated, out int totalCount);
    }
}
