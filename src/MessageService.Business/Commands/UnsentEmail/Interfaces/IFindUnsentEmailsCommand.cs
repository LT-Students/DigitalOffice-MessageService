using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Dto.Responses;

namespace LT.DigitalOffice.MessageService.Business.Commands.UnsentEmail.Interfaces
{
    [AutoInject]
    public interface IFindUnsentEmailsCommand
    {
        UnsentEmailsResponse Execute(int skipCount, int takeCount);
    }
}
