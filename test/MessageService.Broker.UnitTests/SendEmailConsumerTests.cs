using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.UnitTestLibrary;
using LT.DigitalOffice.MessageService.Broker.Consumers;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.Options;
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
        private IOptions<SmtpCredentialsOptions> options;

        private string Subject;
        private string Body;
        private Guid SenderId;
        private string Receiver;

        [SetUp]
        public void SetUp()
        {
            repository = new Mock<IEmailRepository>();
            mapper = new Mock<IMapper<ISendEmailRequest, DbEmail>>();

            options = Options.Create(new SmtpCredentialsOptions());

            options.Value.Host = "smtp.gmail.com";
            options.Value.Port = 587;
            options.Value.Email = "er0289741@gmail.com";
            options.Value.Password = "er0289741123456";

            harness = new InMemoryTestHarness();
            consumerTestHarness = harness.Consumer(() =>
                new SendEmailConsumer(repository.Object, mapper.Object, options));

            Subject = "Title";
            Body = "Content";
            SenderId = Guid.NewGuid();
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
                    SenderId,
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
                    SenderId,
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
