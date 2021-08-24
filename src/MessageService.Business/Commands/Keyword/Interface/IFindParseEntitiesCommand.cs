using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using System.Collections.Generic;

namespace LT.DigitalOffice.MessageService.Business.Commands.ParseEntity.Interface
{
    [AutoInject]
    public interface IFindParseEntitiesCommand
    {
        OperationResultResponse<Dictionary<string, Dictionary<string, List<string>>>> Execute();
    }
}
