using LT.DigitalOffice.MessageService.Business.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.MessageService.Business
{
    public class AddEmailTemplateCommand : IAddEmailTemplateCommand
    {
        private readonly IMapper<EmailTemplate, DbEmailTemplate> mapper;
        private readonly IEmailTemplateRepository repository;

        public AddEmailTemplateCommand(
            [FromServices] IMapper<EmailTemplate, DbEmailTemplate> mapper,
            [FromServices] IEmailTemplateRepository repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        public Guid Execute(EmailTemplate emailTemplate)
        {
            return repository.AddEmailTemplate(mapper.Map(emailTemplate));
        }
    }
}
