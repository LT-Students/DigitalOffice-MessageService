using FluentValidation;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.ChannelUser;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.MessageService.Validation.Validators.ChannelUser.Interface
{
  [AutoInject]
  public interface IEditChannelUserRequestValidator : IValidator<JsonPatchDocument<EditChannelUserRequest>>
  {
  }
}
