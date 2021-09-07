using FluentValidation;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Dto.Requests;

namespace LT.DigitalOffice.MessageService.Validation.Validators.Channel.Interfaces
{
  [AutoInject]
  public interface ICreateChannelRequestValidator : IValidator<CreateChannelRequest>
  {
  }
}
