using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Dto.Helpers;
using LT.DigitalOffice.Models.Broker.Responses.Company;

namespace LT.DigitalOffice.MessageService.Mappers.Models.Interfaces
{
    [AutoInject]
    public interface ISmtpCredentialsMapper
    {
        SmtpCredentials Map(IGetSmtpCredentialsResponse request);
    }
}
