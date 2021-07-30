using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.ParseEntity;
using System;

namespace LT.DigitalOffice.MessageService.Business.Commands.ParseEntity.Interface
{
    [AutoInject]
    public interface IAddKeywordCommand
    {
        OperationResultResponse<Guid> Execute(AddKeywordRequest request);
    }
}
