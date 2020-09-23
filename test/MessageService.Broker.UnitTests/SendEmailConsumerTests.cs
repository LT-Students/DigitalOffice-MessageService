using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.UnitTestLibrary;
using LT.DigitalOffice.MessageService.Broker.Consumers;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto;
using MassTransit;
using MassTransit.Testing;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LT.DigitalOffice.MessageService.Broker.UnitTests
{
    public class SendEmailConsumerTests
    {
        private ConsumerTestHarness<SendEmailConsumer> consumerTestHarness;

        private InMemoryTestHarness harness;
        private Mock<IEmailRepository> repository;
        private Mock<IMapper<ISendEmailRequest, DbEmail>> mapper;
        private IRequestClient<ISendEmailRequest> requestClient;

        private string Subject;
        private string Body;
        private string SenderEmail;
        private string Receiver;

        [SetUp]
        public void SetUp()
        {
            repository = new Mock<IEmailRepository>();
            mapper = new Mock<IMapper<ISendEmailRequest, DbEmail>>();

            harness = new InMemoryTestHarness();
            consumerTestHarness = harness.Consumer(() =>
                new SendEmailConsumer(repository.Object, mapper.Object));

            Subject = "Title";
            Body = "Content";
            SenderEmail = "er0289741 @gmail.com";
            Receiver = "lalagvanan@gmail.com";
        }

        [Test]
        public async Task ShouldSendEmail()
        {
            await harness.Start();

            repository
                .Setup(x => x.SaveEmail(It.IsAny<DbEmail>()));

            try
            {
                requestClient = await harness.ConnectRequestClient<ISendEmailRequest>();

                var response = await requestClient.GetResponse<IOperationResult<bool>>(new
                {
                    SenderEmail,
                    Receiver,
                    Subject,
                    Body
                });

                var expected = new
                {
                    IsSuccess = true,
                    Errors = null as List<string>,
                    Body = true
                };

                SerializerAssert.AreEqual(expected, response.Message);
            }

            finally
            {
                await harness.Stop();
            }
        }

        [Test]
        public async Task ShouldResponseIOperationResultWithExceptionWhenMapperThrowException()
        {
            await harness.Start();

            mapper
                .Setup(x => x.Map(It.IsAny<ISendEmailRequest>()))
                .Throws(new ArgumentNullException());

            try
            {
                requestClient = await harness.ConnectRequestClient<ISendEmailRequest>();


                var response = await requestClient.GetResponse<IOperationResult<bool>>(new
                {
                    SenderEmail,
                    Receiver,
                    Subject,
                    Body
                });

                var expected = new
                {
                    IsSuccess = false,
                    Errors = new List<string> { "Value cannot be null." },
                    Body = false
                };

                SerializerAssert.AreEqual(expected, response.Message);
            }

            finally
            {
                await harness.Stop();
            }
        }
    }
}
