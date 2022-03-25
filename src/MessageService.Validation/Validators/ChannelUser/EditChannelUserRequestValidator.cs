using FluentValidation;
using FluentValidation.Validators;
using LT.DigitalOffice.Kernel.Validators;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.ChannelUser;
using LT.DigitalOffice.MessageService.Validation.Validators.ChannelUser.Interface;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace LT.DigitalOffice.MessageService.Validation.Validators.ChannelUser
{
  public class EditChannelUserRequestValidator
    : BaseEditRequestValidator<EditChannelUserRequest>, IEditChannelUserRequestValidator
  {
    private void HandleInternalPropertyValidation(
      Operation<EditChannelUserRequest> requestedOperation, CustomContext context)
    {
      Context = context;
      RequestedOperation = requestedOperation;

      #region Paths

      AddСorrectPaths(
        new()
        {
          nameof(EditChannelUserRequest.IsActive),
          nameof(EditChannelUserRequest.IsAdmin)
        });

      AddСorrectOperations(nameof(EditChannelUserRequest.IsActive), new() { OperationType.Replace });
      AddСorrectOperations(nameof(EditChannelUserRequest.IsAdmin), new() { OperationType.Replace });

      #endregion

      #region IsActive

      AddFailureForPropertyIf(
        nameof(EditChannelUserRequest.IsActive),
        x => x == OperationType.Replace,
        new()
        {
          { x => bool.TryParse(x.value?.ToString(), out _), "Incorrect IsActive format." },
        });

      #endregion

      #region IsAdmin

      AddFailureForPropertyIf(
        nameof(EditChannelUserRequest.IsAdmin),
        x => x == OperationType.Replace,
        new()
        {
          { x => bool.TryParse(x.value?.ToString(), out _), "Incorrect IsAdmin format." },
        });

      #endregion
    }

    public EditChannelUserRequestValidator()
    {
      RuleForEach(x => x.Operations)
        .Custom(HandleInternalPropertyValidation);
    }
  }
}
