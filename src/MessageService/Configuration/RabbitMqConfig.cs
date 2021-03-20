using LT.DigitalOffice.Kernel.Broker;

namespace LT.DigitalOffice.UserService.Configuration
{
    public class RabbitMqConfig : BaseRabbitMqOptions
    {
        public string SendEmailEndpoint { get; set; }
    }
}
