using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using System;

namespace LT.DigitalOffice.MessageService.Business.UnsentEmailCommands.Interfaces
{
    [AutoInject]
    public interface IResendEmailCommand
    {
        OperationResultResponse<bool> Execute(Guid id);
    }
}
