using LT.DigitalOffice.Kernel.AccessValidator.Interfaces;
using LT.DigitalOffice.MessageService.Business.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.MessageService.Business
{
    public class RemoveEmailTemplateCommand : IRemoveEmailTemplateCommand
    {
        private readonly IEmailTemplateRepository repository;
        private readonly IAccessValidator accessValidator;
        private readonly int numberRight = 3;

        public RemoveEmailTemplateCommand(
            [FromServices] IEmailTemplateRepository repository,
            [FromServices] IAccessValidator accessValidator)
        {
            this.repository = repository;
            this.accessValidator = accessValidator;
        }

        public void Execute(Guid emailTemplateId, Guid requestingUserId)
        {
            var isAcces = GetResultCheckingUserRights(requestingUserId);

            if (!isAcces)
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
