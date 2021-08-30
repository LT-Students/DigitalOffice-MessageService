using FluentValidation;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.MessageService.Validation.Validators.Workspace.Interfaces
{
  [AutoInject]
  public interface IEditWorkspaceRequestValidator : IValidator<JsonPatchDocument<EditWorkspaceRequest>>
  {
  }
}
