using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using LT.DigitalOffice.Kernel.Extensions;
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

    private List<string> AllowedExtensions = new()
    { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tga" };

    private bool AreUsersInWorkspace(List<Guid> requestWorkspaceUsersIds, Guid workspaceId)
    {
      return _workspaceUserRepository
        .DoExistWorkspaceUsers(requestWorkspaceUsersIds, workspaceId);
    }

    private Guid GetCreatorWorkspaceUserId(Guid workspaceId)
    {
      return _workspaceUserRepository
        .Get(workspaceId, _httpContextAccessor.HttpContext.GetUserId()).Id;
    }

    public CreateChannelRequestValidator(
      IWorkspaceUserRepository workspaceUserRepository,
      IHttpContextAccessor httpContextAccessor)
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
          .Must(w => w.Users.Select(i => i.WorkspaceUserId).ToHashSet().Count() == w.Users.Count())
          .WithMessage("A user cannot be added to the channel twice.")
          .Must(w =>
          {
            Guid creatorWorkspaceUserId = GetCreatorWorkspaceUserId(w.WorkspaceId);

            return w.Users.FirstOrDefault(i => i.WorkspaceUserId == creatorWorkspaceUserId) == null;
          })
          .WithMessage("User can not add himself to request users list.")
          .Must(w => AreUsersInWorkspace(w.Users.Select(u => u.WorkspaceUserId).ToList(), w.WorkspaceId))
          .WithMessage("Some users are not available for adding to the channel.");
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
          .WithMessage($"Image extension is not {string.Join('/', AllowedExtensions)}.");
      });
    }
  }
}
