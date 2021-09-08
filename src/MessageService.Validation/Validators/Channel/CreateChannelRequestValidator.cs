using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Models.Dto.Requests;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Channel;
using LT.DigitalOffice.MessageService.Validation.Validators.Channel.Interfaces;

namespace LT.DigitalOffice.MessageService.Validation.Validators.Channel
{
  public class CreateChannelRequestValidator : AbstractValidator<CreateChannelRequest>, ICreateChannelRequestValidator
  {
    private readonly IWorkspaceUserRepository _workspaceUserRepository;
    private List<Guid> _workspaceUsersIds = new();

    private List<string> AllowedExtensions = new()
    { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tga" };

    private bool AreUsersInWorkspace(List<Guid> requestWorkspaceUsersIds, Guid workspaceId, out List<Guid> existingWorkspaceUsersIds)
    {
      existingWorkspaceUsersIds = _workspaceUserRepository
        .AreExistWorkspaceUsers(requestWorkspaceUsersIds, workspaceId);

      if (existingWorkspaceUsersIds.Count != existingWorkspaceUsersIds.Count)
      {
        return false;
      }

      return true;
    }

    public CreateChannelRequestValidator(IWorkspaceUserRepository workspaceUserRepository)
    {
      _workspaceUserRepository = workspaceUserRepository;

      RuleFor(x => x.WorkspaceId)
        .NotEmpty().WithMessage("Workspase id must not be empty");

      RuleFor(x => x.Name.Trim())
        .NotEmpty().WithMessage("Channel name must not be empty");

      RuleFor(x => x.Users)
        .NotNull().WithMessage("Users can not be null");

      When(x => x.Users.Count > 0, () =>
      {
        RuleFor(x => x)
          .Must(x => !AreUsersInWorkspace(
            x.Users.Select(u => u.WorkspaceUserId).ToList(),
            x.WorkspaceId,
            out _workspaceUsersIds))
          .WithMessage($"Only workspace users with ids: {string.Join(", ", _workspaceUsersIds)} available for adding to the channel.");
      });

      When(w => w.Image != null, () =>
      {
        RuleFor(c => c.Image.Content)
          .NotEmpty().WithMessage("Image content can not be empty.")
          .Must(x =>
          {
            try
            {
              var byteString = new Span<byte>(new byte[x.Length]);
              return Convert.TryFromBase64String(x, byteString, out _);
            }
            catch
            {
              return false;
            }
          }).WithMessage("Wrong image content.");

        RuleFor(c => c.Image.Extension)
          .Must(AllowedExtensions.Contains)
          .WithMessage($"Image extension is not {string.Join('/', AllowedExtensions)}");
      });
    }
  }
}
