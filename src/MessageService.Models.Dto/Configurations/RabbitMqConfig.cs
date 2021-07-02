using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Configurations;
using LT.DigitalOffice.Models.Broker.Requests.File;

namespace LT.DigitalOffice.MessageService.Models.Dto.Configurations
{
    public class RabbitMqConfig : BaseRabbitMqConfig
    {
        public string SendEmailEndpoint { get; set; }
        public string CreateSMTPEndpoint { get; set; }

        [AutoInjectRequest(typeof(IAddImageRequest))]
        public string AddImageEndpoint { get; set; }

        public string CreateWorkspaceEndpoint { get; set; }
    }
}
