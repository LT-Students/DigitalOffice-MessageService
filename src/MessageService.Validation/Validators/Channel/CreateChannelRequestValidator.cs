using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    private IHttpContextAccessor _httpContextAccessor;
    private readonly IWorkspaceUserRepository _workspaceUserRepository;

    private async Task<bool> AreUsersInWorkspace(List<Guid> requestUsersIds, Guid workspaceId)
    {
      return await _workspaceUserRepository
        .WorkspaceUsersExist(requestUsersIds, workspaceId);
    }

    public CreateChannelRequestValidator(
      IWorkspaceUserRepository workspaceUserRepository,
      IHttpContextAccessor httpContextAccessor,
      IImageContentValidator imageContentValidator,
      IImageExtensionValidator imageExtensionValidator)
    {
      _httpContextAccessor = httpContextAccessor;
      _workspaceUserRepository = workspaceUserRepository;

      RuleFor(x => x.WorkspaceId)
        .NotEmpty().WithMessage("Workspase id must not be empty.");

      RuleFor(x => x.Name.Trim())
        .NotEmpty().WithMessage("Channel name must not be empty.");

      RuleFor(x => x.Users)
        .NotNull().WithMessage("Users can not be null.");

      When(x => x.Users != null && x.Users.Count > 0, () =>
      {
        RuleFor(w => w)
          .Must(w => w.Users.Select(i => i.UserId).ToHashSet().Count() == w.Users.Count())
          .WithMessage("A user cannot be added to the channel twice.")
          .Must(w =>
          {
            return w.Users.FirstOrDefault(i => i.UserId == _httpContextAccessor.HttpContext.GetUserId()) == null;
          })
          .WithMessage("User can not add himself to request users list.")
          .MustAsync(async (w, _) => await AreUsersInWorkspace(w.Users.Select(u => u.UserId).ToList(), w.WorkspaceId))
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
