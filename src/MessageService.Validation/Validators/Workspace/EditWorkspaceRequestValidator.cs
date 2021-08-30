using FluentValidation;
using FluentValidation.Validators;
using LT.DigitalOffice.MessageService.Models.Dto.Requests;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace;
using LT.DigitalOffice.MessageService.Validation.Helper;
using LT.DigitalOffice.MessageService.Validation.Validators.Workspace.Interfaces;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Newtonsoft.Json;

namespace LT.DigitalOffice.MessageService.Validation.Validators.Workspace
{
  public class EditWorkspaceRequestValidator : BaseEditRequestValidator<EditWorkspaceRequest>, IEditWorkspaceRequestValidator
  {
    private void HandleInternalPropertyValidation(Operation<EditWorkspaceRequest> requestedOperation, CustomContext context)
    {
      Context = context;
      RequestedOperation = requestedOperation;

      #region Paths

      AddСorrectPaths(
        new()
        {
          nameof(EditWorkspaceRequest.Name),
          nameof(EditWorkspaceRequest.Description),
          nameof(EditWorkspaceRequest.Image),
          nameof(EditWorkspaceRequest.IsActive)
        });

      AddСorrectOperations(nameof(EditWorkspaceRequest.Name), new() { OperationType.Replace });
      AddСorrectOperations(nameof(EditWorkspaceRequest.Description), new() { OperationType.Replace });
      AddСorrectOperations(nameof(EditWorkspaceRequest.IsActive), new() { OperationType.Replace });
      AddСorrectOperations(nameof(EditWorkspaceRequest.Image), new() { OperationType.Replace });

      #endregion

      #region Name

      AddFailureForPropertyIf(
          nameof(EditWorkspaceRequest.Name),
          x => x == OperationType.Replace,
          new()
          {
            { x => !string.IsNullOrEmpty(x.value.ToString()), "Name cannot be empty." },
          });

      #endregion

      #region Image

      AddFailureForPropertyIf(
          nameof(EditWorkspaceRequest.Image),
          x => x == OperationType.Replace,
          new()
          {
            {
              x =>
              {
                try
                {
                  _ = JsonConvert.DeserializeObject<CreateImageRequest>(x.value?.ToString());
                  return true;
                }
                catch
                {
                  return false;
                }
              },
              "Incorrect Image format"
            }
          });

      #endregion

      #region IsActive

      AddFailureForPropertyIf(
          nameof(EditWorkspaceRequest.IsActive),
          x => x == OperationType.Replace,
          new()
          {
            { x => bool.TryParse(x.value?.ToString(), out _), "Incorrect IsActive format." },
          });

      #endregion
    }

    public EditWorkspaceRequestValidator()
    {
      RuleForEach(x => x.Operations)
         .Custom(HandleInternalPropertyValidation);
    }
  }
}
