using FluentValidation;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace;

namespace LT.DigitalOffice.MessageService.Validation.Validators.Workspace.Interfaces
{
  [AutoInject]
  public interface ICreateWorkspaceRequestValidator : IValidator<CreateWorkspaceRequest>
  {
  }
}
