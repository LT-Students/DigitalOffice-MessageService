using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Business.UnsentEmailCommands.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using System;

namespace LT.DigitalOffice.MessageService.Business.UnsentEmailCommands
{
    public class ResendCommand : IResendCommand
    {
        private readonly IAccessValidator _accessValidator;
        private readonly IUnsentEmailRepository _repository;

        public ResendCommand(
            IAccessValidator accessValidator,
            IUnsentEmailRepository repository)
        {
            _accessValidator = accessValidator;
            _repository = repository;
        }

        public OperationResultResponse<bool> Execute(Guid id)
        {
            if (!_accessValidator.IsAdmin())
            {
                throw new ForbiddenException("Not enough rights.");
            }

            return new OperationResultResponse<bool>
            {
                Status = OperationResultStatusType.FullSuccess,
                Body = true
            };
        }
    }
}
