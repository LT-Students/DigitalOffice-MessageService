using FluentValidation;
using LT.DigitalOffice.MessageService.Broker.Helpers.ParseEntity;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.ParseEntity;
using LT.DigitalOffice.MessageService.Validation.ParseEntity.Interfaces;

namespace LT.DigitalOffice.MessageService.Validation.ParseEntity
{
    public class AddKeywordRequestValidator : AbstractValidator<AddKeywordRequest>, IAddKeywordRequestValidator
    {
        public AddKeywordRequestValidator(IKeywordRepository repository)
        {
            RuleFor(x => x.Keyword)
                .NotEmpty()
                .MaximumLength(50)
                .Must(k => !repository.DoesKeywordExist(k))
                .WithMessage("This keyword already exists.");

            RuleFor(x => x.ServiceName)
                .IsInEnum();

            RuleFor(x => x.EntityName)
                .NotEmpty();

            RuleFor(x => x.PropertyName)
                .NotEmpty();

            RuleFor(x => x)
                .Must(x => AllParseEntities.Entities.ContainsKey(x.ServiceName.ToString())
                        && AllParseEntities.Entities[x.ServiceName.ToString()].ContainsKey("Db" + x.EntityName)
                        && AllParseEntities.Entities[x.ServiceName.ToString()]["Db" + x.EntityName].Contains(x.PropertyName))
                .WithMessage("No entity with requested property in the service.");
        }
    }
}
