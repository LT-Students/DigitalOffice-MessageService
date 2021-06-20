using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.EmailTemplate;

namespace LT.DigitalOffice.MessageService.Business.Commands.EmailTemplate.Interfaces
{
    /// <summary>
    /// Represents interface for a command in command pattern.
    /// Provides method for adding an email template.
    /// </summary>
    [AutoInject]
    public interface IEditEmailTemplateCommand
    {
        /// <summary>
        /// Edit email template data with check right user.
        /// </summary>
        /// <param name="editEmailTemplate">Edit email template data.</param>
        OperationResultResponse<bool> Execute(EditEmailTemplateRequest editEmailTemplate);
    }
}
