using System;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Validators;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.EmailTemplate;
using LT.DigitalOffice.MessageService.Validation.Helper;
using LT.DigitalOffice.MessageService.Validation.Validators.EmailTemplate.Interfaces;
using LT.DigitalOffice.Models.Broker.Enums;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace LT.DigitalOffice.MessageService.Validation.EmailTemplate
{
  public class EditEmailTemplateValidator : BaseEditRequestValidator<EditEmailTemplateRequest>, IEditEmailTemplateValidator
  {
    private void HandleInternalPropertyValidation(Operation<EditEmailTemplateRequest> requestedOperation, CustomContext context)
    {
      Context = context;
      RequestedOperation = requestedOperation;

      #region Paths

      AddСorrectPaths(
        new List<string>
        {
          nameof(EditEmailTemplateRequest.Name),
          nameof(EditEmailTemplateRequest.Type),
          nameof(EditEmailTemplateRequest.IsActive)
        });

      AddСorrectOperations(nameof(EditEmailTemplateRequest.Name), new List<OperationType> { OperationType.Replace });
      AddСorrectOperations(nameof(EditEmailTemplateRequest.Type), new List<OperationType> { OperationType.Replace });
      AddСorrectOperations(nameof(EditEmailTemplateRequest.IsActive), new List<OperationType> { OperationType.Replace });

      #endregion

      #region Name

      AddFailureForPropertyIf(
        nameof(EditEmailTemplateRequest.Name),
        x => x == OperationType.Replace,
        new()
        {
          { x => !string.IsNullOrEmpty(x.value.ToString()), "Email template name must not be empty." },
        });

      #endregion

      #region Type

      AddFailureForPropertyIf(
        nameof(EditEmailTemplateRequest.Type),
        x => x == OperationType.Replace,
        new()
        {
          { x => Enum.TryParse(typeof(EmailTemplateType), x.value.ToString(), true, out _), "Incorrect Email template type." },
        });

      #endregion

      #region IsActive

      AddFailureForPropertyIf(
        nameof(EditEmailTemplateRequest.IsActive),
        x => x == OperationType.Replace,
        new()
        {
          { x => bool.TryParse(x.value?.ToString(), out _), "Incorrect is active value." },
        });

      #endregion
    }

    public EditEmailTemplateValidator()
    {
      RuleForEach(x => x.Operations).Custom(HandleInternalPropertyValidation);
    }
  }
}
