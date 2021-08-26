using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Business.Commands.EmailTemplate.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Db.Interfaces;
using LT.DigitalOffice.MessageService.Models.Dto.Enums;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.EmailTemplate;
using LT.DigitalOffice.MessageService.Validation.EmailTemplate.Interfaces;
using System;
using System.Linq;

namespace LT.DigitalOffice.MessageService.Business.Commands.EmailTemplate
{
    public class EditEmailTemplateCommand : IEditEmailTemplateCommand
    {
        private readonly IAccessValidator _accessValidator;
        private readonly IEmailTemplateRepository _repository;
        private readonly IEditEmailTemplateValidator _validator;
        private readonly IDbEmailTemplateTextMapper _mapperTemplateText;
        private readonly IEditDbEmailTemplateMapper _mapperEmailTemplate;

        public EditEmailTemplateCommand(
            IAccessValidator accessValidator,
            IEmailTemplateRepository repository,
            IEditEmailTemplateValidator validator,
            IDbEmailTemplateTextMapper mapperTemplateText,
            IEditDbEmailTemplateMapper mapperEmailTemplate)
        {
            _validator = validator;
            _repository = repository;
            _accessValidator = accessValidator;
            _mapperTemplateText = mapperTemplateText;
            _mapperEmailTemplate = mapperEmailTemplate;
        }

        // TODO: rework edit method => patch
        public OperationResultResponse<bool> Execute(EditEmailTemplateRequest editEmailTemplate)
        {
            if (!(_accessValidator.IsAdmin() || _accessValidator.HasRights(Rights.AddEditRemoveEmailTemplates)))
            {
                throw new ForbiddenException("Not enough rights.");
            }

            _validator.ValidateAndThrowCustom(editEmailTemplate);

            var dbEmailTemplate = _repository.Get(editEmailTemplate.Id);

            var editDbEmailTemplate = _mapperEmailTemplate.Map(editEmailTemplate);

            editDbEmailTemplate.CreatedAtUtc = dbEmailTemplate.CreatedAtUtc;
            editDbEmailTemplate.IsActive = dbEmailTemplate.IsActive;
            editDbEmailTemplate.CreatedBy = dbEmailTemplate.CreatedBy;

            foreach (var emailTemplateText in editEmailTemplate.EmailTemplateTexts)
            {
                var dbEmailTemplateTexts = dbEmailTemplate.EmailTemplateTexts
                    .FirstOrDefault(x => x.Language == emailTemplateText.Language);
                var newDbEmailTemplateTexts = _mapperTemplateText.Map(emailTemplateText);

                if (dbEmailTemplateTexts != null)
                {
                    newDbEmailTemplateTexts.Id = dbEmailTemplateTexts.Id;
                }
                else
                {
                    newDbEmailTemplateTexts.Id = Guid.NewGuid();
                }

                newDbEmailTemplateTexts.EmailTemplateId = dbEmailTemplate.Id;
                editDbEmailTemplate.EmailTemplateTexts.Add(newDbEmailTemplateTexts);
            }

            var isSuccess = _repository.Edit(editDbEmailTemplate);

            return new OperationResultResponse<bool>
            {
                Status = isSuccess ? OperationResultStatusType.FullSuccess : OperationResultStatusType.Failed,
                Body = isSuccess
            };
        }
    }
}
