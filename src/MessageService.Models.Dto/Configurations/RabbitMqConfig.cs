using LT.DigitalOffice.Kernel.Configurations;

namespace LT.DigitalOffice.MessageService.Models.Dto.Configurations
{
    public class RabbitMqConfig : BaseRabbitMqConfig
    {
        public string SendEmailEndpoint { get; set; }
        public string CreateImageEndpoint { get; set; }
        public string GetTempalateTagsEndpoint { get; set; }
    }
}
