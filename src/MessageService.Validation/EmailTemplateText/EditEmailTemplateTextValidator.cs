using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Validators;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.EmailTemplateText;
using LT.DigitalOffice.MessageService.Validation.EmailTemplateText.Interfaces;
using LT.DigitalOffice.MessageService.Validation.Helper;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace LT.DigitalOffice.MessageService.Validation.EmailTemplateText
{
  public class EditEmailTemplateTextValidator : BaseEditRequestValidator<EditEmailTemplateTextRequest>, IEditEmailTemplateTextValidator
  {
    private void HandleInternalPropertyValidation(Operation<EditEmailTemplateTextRequest> requestedOperation, CustomContext context)
    {
      Context = context;
      RequestedOperation = requestedOperation;

      #region Paths

      AddСorrectPaths(
        new List<string>
        {
          nameof(EditEmailTemplateTextRequest.Subject),
          nameof(EditEmailTemplateTextRequest.Text),
          nameof(EditEmailTemplateTextRequest.Language)
        });

      AddСorrectOperations(nameof(EditEmailTemplateTextRequest.Subject), new List<OperationType> { OperationType.Replace });
      AddСorrectOperations(nameof(EditEmailTemplateTextRequest.Text), new List<OperationType> { OperationType.Replace });
      AddСorrectOperations(nameof(EditEmailTemplateTextRequest.Language), new List<OperationType> { OperationType.Replace });

      #endregion

      #region Subject

      AddFailureForPropertyIf(
        nameof(EditEmailTemplateTextRequest.Subject),
        x => x == OperationType.Replace,
        new()
        {
          { x => !string.IsNullOrEmpty(x.value.ToString()), "Subject must not be empty." },
        });

      #endregion

      #region Text

      AddFailureForPropertyIf(
        nameof(EditEmailTemplateTextRequest.Text),
        x => x == OperationType.Replace,
        new()
        {
          { x => !string.IsNullOrEmpty(x.value.ToString()), "Text must not be empty." },
        });

      #endregion

      #region Language

      AddFailureForPropertyIf(
        nameof(EditEmailTemplateTextRequest.Language),
        x => x == OperationType.Replace,
        new()
        {
          { x => !string.IsNullOrEmpty(x.value.ToString()), "Language must not be empty." },
          { x => x.value.ToString().Length < 2, "Language must not be empty." },
        });

      #endregion
    }

    public EditEmailTemplateTextValidator()
    {
      RuleForEach(x => x.Operations).Custom(HandleInternalPropertyValidation);
    }
  }
}
