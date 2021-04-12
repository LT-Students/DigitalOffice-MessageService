using FluentValidation;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.MessageService.Business.EmailTemplatesCommands.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.MessageService.Business.EmailTemplatesCommands
{
    public class AddEmailTemplateCommand : IAddEmailTemplateCommand
    {
        private readonly IMapper<EmailTemplateRequest, DbEmailTemplate> mapper;
        private readonly IValidator<EmailTemplateRequest> validator;
        private readonly IEmailTemplateRepository repository;
        private readonly IAccessValidator accessValidator;

        public AddEmailTemplateCommand(
            IMapper<EmailTemplateRequest, DbEmailTemplate> mapper,
            IValidator<EmailTemplateRequest> validator,
            IEmailTemplateRepository repository,
            IAccessValidator accessValidator)
        {
            this.mapper = mapper;
            this.validator = validator;
            this.repository = repository;
            this.accessValidator = accessValidator;
        }

        public Guid Execute(EmailTemplateRequest emailTemplate)
        {
            const int rightId = 3;

            if (!(accessValidator.IsAdmin() || accessValidator.HasRights(rightId)))
            {
                throw new Exception("Not enough rights.");
            }

            validator.ValidateAndThrowCustom(emailTemplate);

            return repository.AddEmailTemplate(mapper.Map(emailTemplate));
        }
    }
}
