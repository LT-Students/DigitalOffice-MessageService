using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Business.Commands.EmailTemplate.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Db.Interfaces;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.EmailTemplate;
using LT.DigitalOffice.MessageService.Validation.Validators.EmailTemplate.Interfaces;
using System;

namespace LT.DigitalOffice.MessageService.Business.Commands.EmailTemplate
{
  public class CreateEmailTemplateCommand : ICreateEmailTemplateCommand
    {
        private readonly IDbEmailTemplateMapper _mapper;
        private readonly IAccessValidator _accessValidator;
        private readonly IEmailTemplateRepository _repository;
        private readonly ICreateEmailTemplateValidator _validator;

        public CreateEmailTemplateCommand(
            IDbEmailTemplateMapper mapper,
            IAccessValidator accessValidator,
            IEmailTemplateRepository repository,
            ICreateEmailTemplateValidator validator)
        {
            _mapper = mapper;
            _validator = validator;
            _repository = repository;
            _accessValidator = accessValidator;
        }

        public OperationResultResponse<Guid> Execute(EmailTemplateRequest emailTemplate)
        {
            if (!(_accessValidator.IsAdmin() || _accessValidator.HasRights(Rights.AddEditRemoveEmailTemplates)))
            {
                throw new ForbiddenException("Not enough rights.");
            }

            _validator.ValidateAndThrowCustom(emailTemplate);

            var id = _repository.Add(_mapper.Map(emailTemplate));

            return new OperationResultResponse<Guid>
            {
                Status = OperationResultStatusType.FullSuccess,
                Body = id
            };
        }
    }
}
