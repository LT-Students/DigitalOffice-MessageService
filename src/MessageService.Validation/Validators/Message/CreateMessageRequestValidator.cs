using FluentValidation;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Message;
using LT.DigitalOffice.MessageService.Validation.Validators.Message.Interfaces;

namespace LT.DigitalOffice.MessageService.Validation.Validators.Message
{
  public class CreateMessageRequestValidator : AbstractValidator<CreateMessageRequest>, ICreateMessageRequestValidator
  {
    public CreateMessageRequestValidator()
    {
      When(request => request.Content != null, () =>
      {
        RuleForEach(request => request.Content)
          .NotEmpty().WithMessage("Content can not be empty.");
      });

      RuleFor(request => request.Status)
        .IsInEnum();
    }
  }
}
