using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.MessageService.Business.EmailTemplatesCommands.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.MessageService.Business.EmailTemplatesCommands
{
    public class AddEmailTemplateCommand : IAddEmailTemplateCommand
    {
        private readonly IMapper<EmailTemplate, DbEmailTemplate> mapper;
        private readonly IEmailTemplateRepository repository;
        private readonly IAccessValidator accessValidator;

        public AddEmailTemplateCommand(
            [FromServices] IMapper<EmailTemplate, DbEmailTemplate> mapper,
            [FromServices] IEmailTemplateRepository repository,
            [FromServices] IAccessValidator accessValidator)
        {
            this.mapper = mapper;
            this.repository = repository;
            this.accessValidator = accessValidator;
        }

        public Guid Execute(EmailTemplate emailTemplate)
        {
            const int rightId = 3;

            if (!(accessValidator.IsAdmin() || accessValidator.HasRights(rightId)))
            {
                throw new Exception("Not enough rights.");
            }

            return repository.AddEmailTemplate(mapper.Map(emailTemplate));
        }
    }
}
