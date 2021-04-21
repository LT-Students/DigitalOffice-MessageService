using FluentValidation;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace;
using LT.DigitalOffice.MessageService.Validation.Workspace.Interfaces;

namespace LT.DigitalOffice.MessageService.Validation.Workspace
{
    public class CreateWorkspaceValidator : AbstractValidator<WorkspaceRequest>, ICreateWorkspaceValidator
    {
        public CreateWorkspaceValidator()
        {
            RuleFor(workspace => workspace.Name)
                .NotEmpty().WithMessage("Workspace name can not be empty.");
        }
    }
}
