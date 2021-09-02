using FluentValidation;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace;
using LT.DigitalOffice.MessageService.Validation.Validators.Workspace.Interfaces;

namespace LT.DigitalOffice.MessageService.Validation.Validators.Workspace
{
  public class CreateWorkspaceRequestValidator : AbstractValidator<CreateWorkspaceRequest>, ICreateWorkspaceRequestValidator
  {
    public CreateWorkspaceRequestValidator()
    {
      RuleFor(workspace => workspace.Name)
        .NotEmpty().WithMessage("Workspace name can not be empty.");

      When(w => w.Avatar != null, () =>
      {
        RuleFor(w => w.Avatar.Content)
          .NotEmpty().WithMessage("Avatar content can not be empty.");
        RuleFor(w => w.Avatar.Extension)
          .NotEmpty().WithMessage("Avatar extension can not be empty.");
      });
    }
  }
}
