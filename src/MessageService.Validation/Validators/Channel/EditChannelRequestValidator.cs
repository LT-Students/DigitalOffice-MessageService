using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Validators;
using LT.DigitalOffice.Kernel.Validators;
using LT.DigitalOffice.Kernel.Validators.Interfaces;
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
          nameof(EditChannelRequest.ImageExtension),
          nameof(EditChannelRequest.ImageContent),
        });

      AddСorrectOperations(nameof(EditChannelRequest.Name), new List<OperationType> { OperationType.Replace });
      AddСorrectOperations(nameof(EditChannelRequest.IsPrivate), new List<OperationType> { OperationType.Replace });
      AddСorrectOperations(nameof(EditChannelRequest.IsActive), new List<OperationType> { OperationType.Replace });
      AddСorrectOperations(nameof(EditChannelRequest.ImageExtension), new List<OperationType> { OperationType.Replace });
      AddСorrectOperations(nameof(EditChannelRequest.ImageContent), new List<OperationType> { OperationType.Replace });

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
        nameof(EditChannelRequest.ImageContent),
        x => x == OperationType.Replace,
        new()
        {
          { x => _imageContentValidator.Validate(x?.value.ToString()).IsValid, "Incorrect format of image content." }
        });

      #endregion

      #region ImageExtension

      AddFailureForPropertyIf(
        nameof(EditChannelRequest.ImageExtension),
        x => x == OperationType.Replace,
        new()
        {
          { x => _imageExtensionValidator.Validate(x?.value.ToString()).IsValid, "Incorrect format of image extension." },
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
