using System;
using System.Collections.Generic;
using FluentValidation;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace;
using LT.DigitalOffice.MessageService.Validation.Validators.Workspace.Interfaces;

namespace LT.DigitalOffice.MessageService.Validation.Validators.Workspace
{
  public class CreateWorkspaceRequestValidator : AbstractValidator<CreateWorkspaceRequest>, ICreateWorkspaceRequestValidator
  {
    private List<string> AllowedExtensions = new()
    { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tga" };

    public CreateWorkspaceRequestValidator()
    {
      RuleFor(workspace => workspace.Name)
        .NotEmpty().WithMessage("Workspace name can not be empty.");

      When(w => w.Image != null, () =>
      {
        RuleFor(w => w.Image.Content)
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

        RuleFor(w => w.Image.Extension)
          .Must(AllowedExtensions.Contains)
          .WithMessage($"Image extension is not {string.Join('/', AllowedExtensions)}");
      });

      RuleFor(w => w.Users)
        .NotEmpty().WithMessage("Users can not be empty");
    }
  }
}
