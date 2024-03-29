﻿using System;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Validators;
using LT.DigitalOffice.Kernel.Validators;
using LT.DigitalOffice.MessageService.Models.Dto.Models.Image;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace;
using LT.DigitalOffice.MessageService.Validation.Validators.Workspace.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Newtonsoft.Json;

namespace LT.DigitalOffice.MessageService.Validation.Validators.Workspace
{
  public class EditWorkspaceRequestValidator : BaseEditRequestValidator<EditWorkspaceRequest>, IEditWorkspaceRequestValidator
  {
    private List<string> AllowedExtensions = new()
    { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tga" };

    private void HandleInternalPropertyValidation(
      Operation<EditWorkspaceRequest> requestedOperation,
      ValidationContext<JsonPatchDocument<EditWorkspaceRequest>> context)
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
                ImageConsist image = JsonConvert.DeserializeObject<ImageConsist>(x.value?.ToString());

                var byteString = new Span<byte>(new byte[image.Content.Length]);

                if (!String.IsNullOrEmpty(image.Content) &&
                  Convert.TryFromBase64String(image.Content, byteString, out _) &&
                  AllowedExtensions.Contains(image.Extension))
                {
                  return true;
                }
              }
              catch
              {
              }
              return false;
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
