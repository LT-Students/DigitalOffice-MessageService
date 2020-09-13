using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.MessageService.Broker.Consumers;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using MassTransit;
using MassTransit.Testing;
using Moq;

namespace LT.DigitalOffice.MessageService.Broker.UnitTests
{
    public class SendEmailConsumerTests
    {
        private ConsumerTestHarness<SendEmailConsumer> consumerTestHarness;

        private InMemoryTestHarness harness;
        private Mock<IMessageRepository> repository;
        private IRequestClient<ISendEmailRequest> requestClient;
    }
}
