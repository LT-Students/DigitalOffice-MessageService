using FluentValidation;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace;
using LT.DigitalOffice.MessageService.Validation.Workspace.Interfaces;

namespace LT.DigitalOffice.MessageService.Validation.Workspace
{
  public class CreateWorkspaceValidator : AbstractValidator<CreateWorkspaceRequest>, ICreateWorkspaceValidator
  {
    public CreateWorkspaceValidator()
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
