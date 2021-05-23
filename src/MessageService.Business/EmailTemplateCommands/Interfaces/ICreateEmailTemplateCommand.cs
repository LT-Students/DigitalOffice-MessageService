using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.EmailTemplate;
using LT.DigitalOffice.MessageService.Models.Dto.Responses;
using System;

namespace LT.DigitalOffice.MessageService.Business.EmailTemplatesCommands.Interfaces
{
    /// <summary>
    /// Represents interface for a command in command pattern.
    /// Provides method for adding an email template.
    /// </summary>
    [AutoInject]
    public interface ICreateEmailTemplateCommand
    {
        /// <summary>
        ///  Adding a new email template.
        /// </summary>
        /// <param name="emailTemplate">Email template data.</param>
        /// <returns>Guid of the added email template.</returns>
        OperationResultResponse<Guid> Execute(EmailTemplateRequest emailTemplate);
    }
}
