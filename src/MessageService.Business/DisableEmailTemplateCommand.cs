using LT.DigitalOffice.Kernel.AccessValidator.Interfaces;
using LT.DigitalOffice.MessageService.Business.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.MessageService.Business
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

        public void Execute(Guid emailTemplateId, Guid requestingUserId)
        {
            var isAccess = GetResultCheckingUserRights(requestingUserId);

            if (!isAccess)
            {
                throw new Exception("Not enough rights.");
            }

            repository.DisableEmailTemplate(emailTemplateId);
        }

        private bool GetResultCheckingUserRights(Guid userId)
        {
            return accessValidator.IsAdmin() || accessValidator.HasRights(numberRight);
        }
    }
}
