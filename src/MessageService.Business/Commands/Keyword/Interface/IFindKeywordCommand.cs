using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Models.Dto.Models;

namespace LT.DigitalOffice.MessageService.Business.Commands.ParseEntity.Interface
{
    [AutoInject]
    public interface IFindKeywordCommand
    {
        FindResultResponse<KeywordInfo> Execute(int skipCount, int takeCount);
    }
}
