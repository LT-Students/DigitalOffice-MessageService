using LT.DigitalOffice.MessageService.Models.Db;
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
        /// Adds new email template to the database.
        /// </summary>
        /// <param name="emailTemplate">Email template to add.</param>
        /// <returns>Guid of added email template.</returns>
        Guid AddEmailTemplate(DbEmailTemplate emailTemplate);
    }
}
