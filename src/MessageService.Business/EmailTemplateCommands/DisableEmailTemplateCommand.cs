using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.MessageService.Business.EmailTemplatesCommands.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using System;

namespace LT.DigitalOffice.MessageService.Business.EmailTemplatesCommands
{
    public class DisableEmailTemplateCommand : IDisableEmailTemplateCommand
    {
        private readonly IAccessValidator _accessValidator;
        private readonly IEmailTemplateRepository _repository;

        public DisableEmailTemplateCommand(
            IAccessValidator accessValidator,
            IEmailTemplateRepository repository)
        {
            _repository = repository;
            _accessValidator = accessValidator;
        }

        public void Execute(Guid emailTemplateId)
        {
            if (!(_accessValidator.IsAdmin() || _accessValidator.HasRights(Rights.AddEditRemoveEmailTemplates)))
            {
                throw new ForbiddenException("Not enough rights.");
            }

            _repository.DisableEmailTemplate(emailTemplateId);
        }
    }
}
