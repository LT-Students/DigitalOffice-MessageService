using System;
using System.Collections.Generic;
using System.Text.Json;
using FluentValidation;
using FluentValidation.Validators;
using LT.DigitalOffice.Kernel.Validators;
using LT.DigitalOffice.Kernel.Validators.Interfaces;
using LT.DigitalOffice.MessageService.Models.Dto.Models.Image;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Channel;
using LT.DigitalOffice.MessageService.Validation.Validators.Channel.Interfaces;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace LT.DigitalOffice.MessageService.Validation.Validators.Channel
{
  public class EditChannelRequestValidator : BaseEditRequestValidator<EditChannelRequest>, IEditChannelRequestValidator
  {
    private readonly IImageContentValidator _imageContentValidator;
    private readonly IImageExtensionValidator _imageExtensionValidator;

    private void HandleInternalPropertyValidation(Operation<EditChannelRequest> requestedOperation, CustomContext context)
    {
      Context = context;
      RequestedOperation = requestedOperation;

      #region paths

      AddСorrectPaths(
        new List<string>
        {
          nameof(EditChannelRequest.Name),
          nameof(EditChannelRequest.IsActive),
          nameof(EditChannelRequest.IsPrivate),
          nameof(EditChannelRequest.Image),
        });

      AddСorrectOperations(nameof(EditChannelRequest.Name), new List<OperationType> { OperationType.Replace });
      AddСorrectOperations(nameof(EditChannelRequest.IsPrivate), new List<OperationType> { OperationType.Replace });
      AddСorrectOperations(nameof(EditChannelRequest.IsActive), new List<OperationType> { OperationType.Replace });
      AddСorrectOperations(nameof(EditChannelRequest.Image), new List<OperationType> { OperationType.Replace });

      #endregion

      #region Name

      AddFailureForPropertyIf(
        nameof(EditChannelRequest.Name),
        x => x == OperationType.Replace,
        new()
        {
          { x => !string.IsNullOrEmpty(x.value?.ToString().Trim()), "Name must not be empty." },
        });

      #endregion

      #region IsPrivate

      AddFailureForPropertyIf(
        nameof(EditChannelRequest.IsPrivate),
        x => x == OperationType.Replace,
        new()
        {
          { x => bool.TryParse(x?.value.ToString(), out _), "Incorrect format of 'IsPrivate'." },
        });

      #endregion

      #region IsActive

      AddFailureForPropertyIf(
        nameof(EditChannelRequest.IsActive),
        x => x == OperationType.Replace,
        new()
        {
          { x => bool.TryParse(x?.value.ToString(), out _), "Incorrect format of 'IsActive'." },
        });

      #endregion

      #region ImageContent

      AddFailureForPropertyIf(
        nameof(EditChannelRequest.Image),
        x => x == OperationType.Replace,
        new()
        {
          { x =>
            {
              try
              {
                ImageConsist image = JsonSerializer.Deserialize<ImageConsist>(x.value?.ToString());

                return _imageContentValidator.Validate(image.Content).IsValid
                  && _imageExtensionValidator.Validate(image.Extension).IsValid;
              }
              catch (Exception)
              {
                return false;
              }
            }, "Incorrect format of image." }
        });

      #endregion
    }

    public EditChannelRequestValidator(
      IImageContentValidator imageContentValidator,
      IImageExtensionValidator imageExtensionValidator)
    {
      _imageContentValidator = imageContentValidator;
      _imageExtensionValidator = imageExtensionValidator;

      RuleForEach(x => x.Operations)
        .Custom(HandleInternalPropertyValidation);
    }
  }
}
