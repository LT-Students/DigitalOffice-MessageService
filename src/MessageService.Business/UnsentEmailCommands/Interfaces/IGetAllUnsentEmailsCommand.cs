using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Dto.Responses;

namespace LT.DigitalOffice.MessageService.Business.UnsentEmailCommands.Interfaces
{
    [AutoInject]
    public interface IGetAllUnsentEmailsCommand
    {
        UnsentEmailsResponse Execute();
    }
}
