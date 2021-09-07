using System;
using System.Collections.Generic;
using FluentValidation;
using LT.DigitalOffice.MessageService.Models.Dto.Requests;
using LT.DigitalOffice.MessageService.Validation.Validators.Channel.Interfaces;

namespace LT.DigitalOffice.MessageService.Validation.Validators.Channel
{
  public class CreateChannelRequestValidator : AbstractValidator<CreateChannelRequest>, ICreateChannelRequestValidator
  {
    private List<string> AllowedExtensions = new()
    { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tga" };

    public CreateChannelRequestValidator()
    {
      RuleFor(x => x.WorkspaceId)
        .NotEmpty().WithMessage("Workspase id must not be empty");

      RuleFor(x => x.Name.Trim())
        .NotEmpty().WithMessage("Channel name must not be empty");

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
