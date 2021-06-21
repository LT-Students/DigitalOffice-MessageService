using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Broker.Helpers;
using LT.DigitalOffice.MessageService.Business.Commands.UnsentEmail.Interfaces;
using System;

namespace LT.DigitalOffice.MessageService.Business.Commands.UnsentEmail
{
    public class ResendEmailCommand : IResendEmailCommand
    {
        private readonly IAccessValidator _accessValidator;
        private readonly EmailSender _emailSender;

        public ResendEmailCommand(
            IAccessValidator accessValidator,
            EmailSender emailSender)
        {
            _accessValidator = accessValidator;
            _emailSender = emailSender;
        }

        public OperationResultResponse<bool> Execute(Guid id)
        {
            if (!_accessValidator.IsAdmin())
            {
                throw new ForbiddenException("Not enough rights.");
            }

            bool isSuccess = _emailSender.ResendEmail(id);

            return new OperationResultResponse<bool>
            {
                Status = isSuccess ? OperationResultStatusType.FullSuccess : OperationResultStatusType.Failed,
                Body = isSuccess
            };
        }
    }
}
