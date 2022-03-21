using FluentValidation;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Channel;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.MessageService.Validation.Validators.Channel.Interfaces
{
  [AutoInject]
  public interface IEditChannelRequestValidator : IValidator<JsonPatchDocument<EditChannelRequest>>
  {
  }
}
