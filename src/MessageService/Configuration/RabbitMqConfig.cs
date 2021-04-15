using LT.DigitalOffice.Kernel.Configurations;

namespace LT.DigitalOffice.UserService.Configuration
{
    public class RabbitMqConfig : BaseRabbitMqConfig
    {
        public string SendEmailEndpoint { get; set; }

        public string CreateImageEndpoint { get; set; }
        public string GetTempalateTagsEndpoint { get; set; }
    }
}
