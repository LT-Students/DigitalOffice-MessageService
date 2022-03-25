using FluentValidation;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Message;
using LT.DigitalOffice.MessageService.Validation.Validators.Message.Interfaces;

namespace LT.DigitalOffice.MessageService.Validation.Validators.Message
{
  public class EditMessageRequestValidator : AbstractValidator<EditMessageRequest>, IEditMessageRequestValidator
  {
    public EditMessageRequestValidator()
    {
      When(request => request.Content != null, () =>
      {
        RuleForEach(request => request.Content)
          .NotEmpty()
          .WithMessage("Content can not be empty.");
      });

      RuleFor(request => request.Status)
        .IsInEnum();
    }
  }
}
