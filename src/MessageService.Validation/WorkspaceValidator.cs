using FluentValidation;
using LT.DigitalOffice.MessageService.Models.Dto.Requests;

namespace LT.DigitalOffice.MessageService.Validation
{
    public class WorkspaceValidator : AbstractValidator<Workspace>
    {
        public WorkspaceValidator()
        {
            RuleFor(workspace => workspace.Name)
                .NotEmpty().WithMessage("Workspace name can not be empty.");
        }
    }
}
