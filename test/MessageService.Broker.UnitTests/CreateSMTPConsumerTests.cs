using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.MessageService.Broker.Consumers;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Db.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.Models.Broker.Requests.Message;
using LT.DigitalOffice.UnitTestKernel;
using MassTransit;
using MassTransit.Testing;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.DigitalOffice.MessageService.Broker.UnitTests
{
    public class CreateSMTPConsumerTests
    {
        private InMemoryTestHarness _harness;
        private ConsumerTestHarness<CreateSMTPCredentialsConsumer> _consumerTestHarness;
        private AutoMocker _mocker;
        private IRequestClient<ICreateSMTPRequest> _requestClient;

        private object _request;
        private DbSMTPCredentials _smtp;

        [SetUp]
        public void SetUp()
        {
            _mocker = new AutoMocker();

            _harness = new InMemoryTestHarness();
            _consumerTestHarness = _harness.Consumer(() =>
                _mocker.CreateInstance<CreateSMTPCredentialsConsumer>());

            string host = "host";
            int port = 123;
            bool enableSsl = true;
            string email = "email";
            string password = "password";

            _request = ICreateSMTPRequest.CreateObj(
                host,
                port,
                enableSsl,
                email,
                password);

            _smtp = new DbSMTPCredentials
            {
                Id = Guid.NewGuid(),
                Host = host,
                Email = email,
                EnableSsl = enableSsl,
                Password = password,
                Port = port
            };

            _mocker
                .Setup<IDbSMTPCredentialsMapper, DbSMTPCredentials>(x => x.Map(It.IsAny<ICreateSMTPRequest>()))
                .Returns(_smtp);
        }

        [Test]
        public async Task ShouldThrowExceptionWhenRepositoryThrow()
        {
            _mocker
                .Setup<ISMTPCredentialsRepository>(x => x.Create(It.IsAny<DbSMTPCredentials>()))
                .Throws(new Exception());

            await _harness.Start();

            try
            {
                _requestClient = await _harness.ConnectRequestClient<ICreateSMTPRequest>();

                var response = _requestClient.GetResponse<IOperationResult<bool>>(
                    _request).Result.Message;

                Assert.IsFalse(response.IsSuccess);
                Assert.IsFalse(response.Body);
                Assert.IsNotEmpty(response.Errors);
                Assert.True(_consumerTestHarness.Consumed.Select<ICreateSMTPRequest>().Any());
                Assert.True(_harness.Sent.Select<IOperationResult<bool>>().Any());
                _mocker
                    .Verify<ISMTPCredentialsRepository>(x => x.Create(_smtp), Times.Once);
            }
            finally
            {
                await _harness.Stop();
            }
        }

        [Test]
        public async Task ShouldCreateSMTPSuccessfully()
        {
            await _harness.Start();

            try
            {
                _requestClient = await _harness.ConnectRequestClient<ICreateSMTPRequest>();

                var response = _requestClient.GetResponse<IOperationResult<bool>>(
                    _request).Result.Message;

                Assert.True(response.IsSuccess);
                Assert.True(response.Body);
                Assert.IsNull(response.Errors);
                Assert.True(_consumerTestHarness.Consumed.Select<ICreateSMTPRequest>().Any());
                Assert.True(_harness.Sent.Select<IOperationResult<bool>>().Any());
                _mocker
                    .Verify<ISMTPCredentialsRepository>(x => x.Create(_smtp), Times.Once);
            }
            finally
            {
                await _harness.Stop();
            }
        }
    }
}
