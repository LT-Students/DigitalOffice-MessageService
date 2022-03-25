using FluentValidation;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.WorkspaceUser;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.MessageService.Validation.Validators.WorkspaceUser.Interface
{
  [AutoInject]
  public interface IEditWorkspaceUserRequestValidator : IValidator<JsonPatchDocument<EditWorkspaceUserRequest>>
  {
  }
}
