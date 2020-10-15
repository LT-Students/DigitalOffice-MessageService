﻿using FluentValidation;
using LT.DigitalOffice.Kernel.AccessValidator.Interfaces;
using LT.DigitalOffice.Kernel.Exceptions;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.MessageService.Business.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.MessageService.Business
{
    public class EditEmailTemplateCommand : IEditEmailTemplateCommand
    {
        private readonly IAccessValidator accessValidator;
        private readonly IEmailTemplateRepository repository;
        private readonly IValidator<EditEmailTemplateRequest> validator;
        private readonly IMapper<EditEmailTemplateRequest, DbEmailTemplate, DbEmailTemplate> mapper;

        private readonly int numberRight = 3;

        public EditEmailTemplateCommand(
            [FromServices] IAccessValidator accessValidator,
            [FromServices] IEmailTemplateRepository repository,
            [FromServices] IValidator<EditEmailTemplateRequest> validator,
            [FromServices] IMapper<EditEmailTemplateRequest, DbEmailTemplate, DbEmailTemplate> mapper)
        {
            this.mapper = mapper;
            this.validator = validator;
            this.repository = repository;
            this.accessValidator = accessValidator;
        }

        public void Execute(EditEmailTemplateRequest editEmailTemplate, Guid requestingUser)
        {
            CheckUserRights();

            validator.ValidateAndThrowCustom(editEmailTemplate);

            var dbEmailTemplate = repository.GetEmailTemplateById(editEmailTemplate.Id);

            dbEmailTemplate = mapper.Map(editEmailTemplate, dbEmailTemplate);

            repository.EditEmailTemplateById(dbEmailTemplate);
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
