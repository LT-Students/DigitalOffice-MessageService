using LT.DigitalOffice.MessageService.Models.Db;

namespace LT.DigitalOffice.MessageService.Data.Interfaces
{
    /// <summary>
    /// Represents interface of repository in repository pattern.
    /// Provides methods for working with the database of MessageService.
    /// </summary>
    public interface IMessageRepository
    {
        /// <summary>
        /// Adds a new message to the database.
        /// </summary>
        /// <param name="message">Message to save.</param>
        void SaveEmail(DbEmail email);
    }
}
