using FluentValidation;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.ParseEntity;

namespace LT.DigitalOffice.MessageService.Validation.ParseEntity.Interfaces
{
    [AutoInject]
    public interface IAddKeywordRequestValidator : IValidator<AddKeywordRequest>
    {
    }
}
