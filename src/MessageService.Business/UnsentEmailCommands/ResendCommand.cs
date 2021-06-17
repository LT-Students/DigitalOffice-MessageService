﻿using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Business.UnsentEmailCommands.Interfaces;
using LT.DigitalOffice.UserService.Business.Helpers.Email;
using System;

namespace LT.DigitalOffice.MessageService.Business.UnsentEmailCommands
{
    public class ResendCommand : IResendCommand
    {
        private readonly IAccessValidator _accessValidator;
        private readonly EmailSender _emailSender;

        public ResendCommand(
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
