using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Dto.Responses;

namespace LT.DigitalOffice.MessageService.Business.Commands.EmailTemplate.Interfaces
{
    [AutoInject]
    public interface IFindEmailTemplateCommand
    {
        EmailTemplatesResponse Execute(int skipCount, int takeCount, bool? includeDeactivated);
    }
}
