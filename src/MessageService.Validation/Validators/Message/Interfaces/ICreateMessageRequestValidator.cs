using FluentValidation;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Message;

namespace LT.DigitalOffice.MessageService.Validation.Validators.Message.Interfaces
{
  [AutoInject]
  public interface ICreateMessageRequestValidator : IValidator<CreateMessageRequest>
  {
  }
}
