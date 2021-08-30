using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Models.Dto.Models.Emails;

namespace LT.DigitalOffice.MessageService.Business.Commands.EmailTemplate.Interfaces
{
  [AutoInject]
  public interface IFindEmailTemplateCommand
  {
    FindResultResponse<EmailTemplateInfo> Execute(int skipCount, int takeCount, bool includeDeactivated = false);
  }
}
