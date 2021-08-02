using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Configurations;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using LT.DigitalOffice.Models.Broker.Requests.File;
using System.Collections.Generic;

namespace LT.DigitalOffice.MessageService.Models.Dto.Configurations
{
    public class RabbitMqConfig : BaseRabbitMqConfig
    {
        public string CreateWorkspaceEndpoint { get; set; }
        public string SendEmailEndpoint { get; set; }
        public string UpdateSmtpCredentialsEndpoint { get; set; }
        public Dictionary<string, string> FindUserParseEntitiesEndpoint { get; set; }

        [AutoInjectRequest(typeof(IGetSmtpCredentialsRequest))]
        public string GetSmtpCredentialsEndpoint { get; set; }

        [AutoInjectRequest(typeof(IAddImageRequest))]
        public string AddImageEndpoint { get; set; }

    }
}
