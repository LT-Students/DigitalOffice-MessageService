using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Dto.Configurations;

namespace LT.DigitalOffice.Broker.Requests
{
    [AutoInjectRequest(nameof(RabbitMqConfig.CreateImageEndpoint))]
    public interface IAddImageRequest
    {
        string Content { get; }

        static object CreateObj(string content)
        {
            return new
            {
                Content = content
            };
        }
    }
}
