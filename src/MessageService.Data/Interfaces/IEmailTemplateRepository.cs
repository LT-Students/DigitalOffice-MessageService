using System;
using System.Collections.Generic;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Db;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.MessageService.Data.Interfaces
{
  /// <summary>
  /// Represents interface of repository in repository pattern.
  /// Provides methods for working with EmailTemplate in the database of MessageService.
  /// </summary>
  [AutoInject]
  public interface IEmailTemplateRepository
  {
    Guid? Create(DbEmailTemplate request);

    bool Edit(Guid emailTemplateId, JsonPatchDocument<DbEmailTemplate> request);

    /// <summary>
    /// Get email template by id from the database.
    /// </summary>
    /// <param name="id">Email template id.</param>
    DbEmailTemplate Get(Guid emailTemplateId);

    /// <summary>
    /// Get first email template by type from the database.
    /// </summary>
    /// <param name="type">Email template type.</param>
    DbEmailTemplate Get(int type);

    List<DbEmailTemplate> Find(int skipCount, int takeCount, out int totalCount, List<string> errors, bool includeDeactivated = false);
  }
}
