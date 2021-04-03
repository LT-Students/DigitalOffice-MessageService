using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.MessageService.Business.EmailTemplates.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.MessageService.Business.EmailTemplates
{
    public class DisableEmailTemplateCommand : IDisableEmailTemplateCommand
    {
        private readonly IEmailTemplateRepository repository;
        private readonly IAccessValidator accessValidator;
        private readonly int numberRight = 3;

        public DisableEmailTemplateCommand(
            [FromServices] IEmailTemplateRepository repository,
            [FromServices] IAccessValidator accessValidator)
        {
            this.repository = repository;
            this.accessValidator = accessValidator;
        }

        public void Execute(Guid emailTemplateId)
        {
            if (!(accessValidator.IsAdmin() || accessValidator.HasRights(numberRight)))
            {
                throw new Exception("Not enough rights.");
            }

            repository.DisableEmailTemplate(emailTemplateId);
        }
    }
}
