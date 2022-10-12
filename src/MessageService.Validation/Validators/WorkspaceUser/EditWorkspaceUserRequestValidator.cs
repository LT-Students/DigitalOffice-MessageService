using FluentValidation;
using FluentValidation.Validators;
using LT.DigitalOffice.Kernel.Validators;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.ChannelUser;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.WorkspaceUser;
using LT.DigitalOffice.MessageService.Validation.Validators.WorkspaceUser.Interface;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace LT.DigitalOffice.MessageService.Validation.Validators.ChannelUser
{
  public class EditWorkspaceUserRequestValidator
    : BaseEditRequestValidator<EditWorkspaceUserRequest>, IEditWorkspaceUserRequestValidator
  {
    private void HandleInternalPropertyValidation(
      Operation<EditWorkspaceUserRequest> requestedOperation,
      ValidationContext<JsonPatchDocument<EditWorkspaceUserRequest>> context)
    {
      Context = context;
      RequestedOperation = requestedOperation;

      #region Paths

      AddСorrectPaths(
        new()
        {
          nameof(EditWorkspaceUserRequest.IsActive),
          nameof(EditWorkspaceUserRequest.IsAdmin)
        });

      AddСorrectOperations(nameof(EditWorkspaceUserRequest.IsActive), new() { OperationType.Replace });
      AddСorrectOperations(nameof(EditWorkspaceUserRequest.IsAdmin), new() { OperationType.Replace });

      #endregion

      #region IsActive

      AddFailureForPropertyIf(
        nameof(EditWorkspaceUserRequest.IsActive),
        x => x == OperationType.Replace,
        new()
        {
          { x => bool.TryParse(x.value?.ToString(), out _), "Incorrect IsActive format." },
        });

      #endregion

      #region IsAdmin

      AddFailureForPropertyIf(
        nameof(EditWorkspaceUserRequest.IsAdmin),
        x => x == OperationType.Replace,
        new()
        {
          { x => bool.TryParse(x.value?.ToString(), out _), "Incorrect IsAdmin format." },
        });

      #endregion
    }

    public EditWorkspaceUserRequestValidator()
    {
      RuleForEach(x => x.Operations)
        .Custom(HandleInternalPropertyValidation);
    }
  }
}
