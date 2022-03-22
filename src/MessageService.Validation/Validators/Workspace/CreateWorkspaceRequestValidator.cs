using System.Linq;
using FluentValidation;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.Validators.Interfaces;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace;
using LT.DigitalOffice.MessageService.Validation.Validators.User.Interfaces;
using LT.DigitalOffice.MessageService.Validation.Validators.Workspace.Interfaces;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.MessageService.Validation.Validators.Workspace
{
  public class CreateWorkspaceRequestValidator : AbstractValidator<CreateWorkspaceRequest>, ICreateWorkspaceRequestValidator
  {
    private IHttpContextAccessor _httpContextAccessor;

    public CreateWorkspaceRequestValidator(
      IHttpContextAccessor httpContextAccessor,
      IImageContentValidator imageContentValidator,
      IImageExtensionValidator imageExtensionValidator,
      IUserExistenceValidator userExistenceValidator)
    {
      _httpContextAccessor = httpContextAccessor;

      RuleFor(workspace => workspace.Name)
        .NotEmpty().WithMessage("Workspace name cannot be empty.");

      When(w => w.Image != null, () =>
      {
        RuleFor(w => w.Image.Content)
          .SetValidator(imageContentValidator);

        RuleFor(w => w.Image.Extension)
          .SetValidator(imageExtensionValidator);
      });

      When(w => w.Users != null && w.Users.Count > 0, () =>
      {
        RuleFor(w => w.Users)
          .Must(u => u.ToHashSet().Count == u.Count)
          .WithMessage("A user cannot be added to the workspace twice.")
          .Must(u => !u.Any(userId => userId == _httpContextAccessor.HttpContext.GetUserId()))
          .WithMessage("Creator cannot be added to workspace users request.")
          .SetValidator(userExistenceValidator);
      });
    }
  }
}
