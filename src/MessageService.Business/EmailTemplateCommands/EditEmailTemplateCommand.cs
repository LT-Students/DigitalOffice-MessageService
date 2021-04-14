using FluentValidation;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Exceptions;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.MessageService.Business.EmailTemplatesCommands.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Models;
using LT.DigitalOffice.MessageService.Models.Dto.Requests;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace LT.DigitalOffice.MessageService.Business.EmailTemplatesCommands
{
    public class EditEmailTemplateCommand : IEditEmailTemplateCommand
    {
        private readonly IAccessValidator accessValidator;
        private readonly IEmailTemplateRepository repository;
        private readonly IValidator<EditEmailTemplateRequest> validator;
        private readonly IMapper<EmailTemplateTextInfo, DbEmailTemplateText> mapperTemplateText;
        private readonly IMapper<EditEmailTemplateRequest, DbEmailTemplate> mapperEmailTemplate;

        private readonly int numberRight = 3;

        public EditEmailTemplateCommand(
            [FromServices] IAccessValidator accessValidator,
            [FromServices] IEmailTemplateRepository repository,
            [FromServices] IValidator<EditEmailTemplateRequest> validator,
            [FromServices] IMapper<EmailTemplateTextInfo, DbEmailTemplateText> mapperTemplateText,
            [FromServices] IMapper<EditEmailTemplateRequest, DbEmailTemplate> mapperEmailTemplate)
        {
            this.validator = validator;
            this.repository = repository;
            this.accessValidator = accessValidator;
            this.mapperTemplateText = mapperTemplateText;
            this.mapperEmailTemplate = mapperEmailTemplate;
        }

        // TODO: rework edit method => patch
        public void Execute(EditEmailTemplateRequest editEmailTemplate)
        {
            CheckUserRights();

            validator.ValidateAndThrowCustom(editEmailTemplate);

            var dbEmailTemplate = repository.GetEmailTemplateById(editEmailTemplate.Id);

            var editDbEmailTemplate = mapperEmailTemplate.Map(editEmailTemplate);

            editDbEmailTemplate.CreatedAt = dbEmailTemplate.CreatedAt;
            editDbEmailTemplate.IsActive = dbEmailTemplate.IsActive;
            editDbEmailTemplate.AuthorId = dbEmailTemplate.AuthorId;

            foreach (var emailTemplateText in editEmailTemplate.EmailTemplateTexts)
            {
                var dbEmailTemplateTexts = dbEmailTemplate.EmailTemplateTexts
                    .FirstOrDefault(x => x.Language == emailTemplateText.Language);
                var newDbEmailTemplateTexts = mapperTemplateText.Map(emailTemplateText);

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

            repository.EditEmailTemplate(editDbEmailTemplate);
        }

        private void CheckUserRights()
        {
            bool isAccess = accessValidator.IsAdmin() || accessValidator.HasRights(numberRight);

            if (!isAccess)
            {
                throw new ForbiddenException("Not enough rights.");
            }
        }
    }
}
