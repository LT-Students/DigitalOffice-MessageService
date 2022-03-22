using System.Linq;
using FluentValidation;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.Validators.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Models.Dto.Requests;
using LT.DigitalOffice.MessageService.Validation.Validators.Channel.Interfaces;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.MessageService.Validation.Validators.Channel
{
  public class CreateChannelRequestValidator : AbstractValidator<CreateChannelRequest>, ICreateChannelRequestValidator
  {
    public CreateChannelRequestValidator(
      IWorkspaceUserRepository workspaceUserRepository,
      IHttpContextAccessor httpContextAccessor,
      IImageContentValidator imageContentValidator,
      IImageExtensionValidator imageExtensionValidator)
    {
      RuleFor(x => x.WorkspaceId)
        .NotEmpty()
        .WithMessage("Workspace id must not be empty.");

      RuleFor(x => x.Name.Trim())
        .NotEmpty()
        .WithMessage("Channel name must not be empty.");

      RuleFor(x => x.UsersIds)
        .NotNull()
        .WithMessage("UsersIds cannot be null.")
        .NotEmpty()
        .WithMessage("UsersIds cannot be empty.");

      When(x => x.UsersIds != null && x.UsersIds.Count > 0, () =>
      {
        RuleFor(w => w)
          .Must(w => w.UsersIds.ToHashSet().Count == w.UsersIds.Count)
          .WithMessage("A user cannot be added to the channel twice.")
          .Must(w =>
          {
            return !w.UsersIds.Contains(httpContextAccessor.HttpContext.GetUserId());
          })
          .WithMessage("User can not add himself to request users list.")
          .MustAsync(async (w, _) => await workspaceUserRepository.WorkspaceUsersExistAsync(w.UsersIds, w.WorkspaceId))
          .WithMessage("Some users are not available for adding to the channel.");
      });

      When(w => w.Image != null, () =>
      {
        RuleFor(w => w.Image.Content)
          .SetValidator(imageContentValidator);

        RuleFor(w => w.Image.Extension)
          .SetValidator(imageExtensionValidator);
      });
    }
  }
}
