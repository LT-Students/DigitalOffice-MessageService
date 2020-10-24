using FluentValidation;
using LT.DigitalOffice.MessageService.Models.Dto;

namespace LT.DigitalOffice.MessageService.Validation
{
    public class AddWorkspaceRequestValidator : AbstractValidator<AddWorkspaceRequest>
    {
        public AddWorkspaceRequestValidator()
        {
            RuleFor(workspace => workspace.Name)
                .NotEmpty().WithMessage("Workspace name can not be empty.")
                .NotEqual("").WithMessage("Workspace name can not have no symbols.");
        }
    }
}
