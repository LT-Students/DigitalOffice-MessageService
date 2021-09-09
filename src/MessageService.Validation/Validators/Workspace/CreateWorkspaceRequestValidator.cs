using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace;
using LT.DigitalOffice.MessageService.Validation.Validators.Workspace.Interfaces;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.MessageService.Validation.Validators.Workspace
{
  public class CreateWorkspaceRequestValidator : AbstractValidator<CreateWorkspaceRequest>, ICreateWorkspaceRequestValidator
  {
    private IHttpContextAccessor _httpContextAccessor;

    private List<string> AllowedExtensions = new()
    { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tga" };

    public CreateWorkspaceRequestValidator(
      IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor;

      Guid creatorId = _httpContextAccessor.HttpContext.GetUserId();

      RuleFor(workspace => workspace.Name)
        .NotEmpty().WithMessage("Workspace name cannot be empty.");

      When(w => w.Image != null, () =>
      {
        RuleFor(w => w.Image.Content)
          .NotEmpty().WithMessage("Image content cannot be empty.")
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

        RuleFor(w => w.Image.Extension)
          .Must(AllowedExtensions.Contains)
          .WithMessage($"Image extension is not {string.Join('/', AllowedExtensions)}");
      });

      RuleFor(w => w.Users)
        .NotNull().WithMessage("Users cannot be null");

      When(w => w.Users != null && w.Users.Count > 0, () =>
      {
        RuleFor(w => w.Users)
          .Must(u => u.Select(i => i.UserId).ToHashSet().Count() == u.Count())
          .WithMessage("A user cannot be added to the workspace twice")
          .Must(u => u.FirstOrDefault(i => i.UserId == creatorId) == null)
          .WithMessage($"User id {creatorId} cannot be added to workspace users");
      });
    }
  }
}
