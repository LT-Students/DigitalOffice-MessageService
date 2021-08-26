using FluentValidation;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace;

namespace LT.DigitalOffice.MessageService.Validation.Workspace.Interfaces
{
    [AutoInject]
    public interface ICreateWorkspaceValidator : IValidator<CreateWorkspaceRequest>
    {
    }
}
