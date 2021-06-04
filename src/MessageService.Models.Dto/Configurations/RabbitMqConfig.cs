using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Configurations;
using LT.DigitalOffice.Models.Broker.Requests.File;
using LT.DigitalOffice.Models.Broker.Requests.Message;

namespace LT.DigitalOffice.MessageService.Models.Dto.Configurations
{
    public class RabbitMqConfig : BaseRabbitMqConfig
    {
        [AutoInjectRequest(typeof(ISendEmailRequest))]
        public string SendEmailEndpoint { get; set; }

        [AutoInjectRequest(typeof(IAddImageRequest))]
        public string AddImageEndpoint { get; set; }

        [AutoInjectRequest(typeof(IGetEmailTemplateTagsRequest))]
        public string GetTempalateTagsEndpoint { get; set; }
    }
}
