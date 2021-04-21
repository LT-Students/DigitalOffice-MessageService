using FluentValidation;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.EmailTemplate;

namespace LT.DigitalOffice.MessageService.Validation.EmailTemplate.Interfaces
{
    [AutoInject]
    public interface ICreateEmailTemplateValidator : IValidator<EmailTemplateRequest>
    {
    }
}
