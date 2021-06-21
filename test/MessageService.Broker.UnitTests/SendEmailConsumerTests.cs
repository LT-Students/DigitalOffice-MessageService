using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.MessageService.Broker.Consumers;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.Models.Broker.Requests.Message;
using LT.DigitalOffice.UnitTestKernel;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.Options;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LT.DigitalOffice.MessageService.Broker.UnitTests
{
    class SendEmailConsumerTests
    {
        private Dictionary<string, string> _templateTags;

        private DbEmailTemplate _dbEmailTemplate;
        private InMemoryTestHarness _harness;
        private ConsumerTestHarness<SendEmailConsumer> _consumerTestHarness;
        private AutoMocker _mocker;
        private IRequestClient<ISendEmailRequest> _requestClient;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
           _templateTags = new Dictionary<string, string>();

            _templateTags.Add("userFirstName", "Ivan");
            _templateTags.Add("userPassword", "123FFSdq3");

            var emailId = Guid.NewGuid();

            var dbEmailTemplateText = new DbEmailTemplateText
            {
                Id = Guid.NewGuid(),
                EmailTemplateId = emailId,
                Subject = "Subject",
                Text = "Hello, {userFirstName}!!! Your password: {userPassword}, enter it at login",
                Language = "en"
            };

            _dbEmailTemplate = new DbEmailTemplate
            {
                Id = emailId,
                CreatedAt = DateTime.UtcNow,
                Name = "Pattern name",
                Type = 2,
                AuthorId = Guid.NewGuid(),
                IsActive = true,
                EmailTemplateTexts = new List<DbEmailTemplateText>
                {
                    dbEmailTemplateText
                }
            };
        }

        [SetUp]
        public void SetUp()
        {
            _mocker = new AutoMocker();

            _mocker
                .Setup<IOptions<SmtpCredentialsOptions>, SmtpCredentialsOptions>(x => x.Value)
                .Returns(new SmtpCredentialsOptions
                {
                    Email = "lt.digitaloffice@gmail.com",
                    Host = "smtp.gmail.com",
                    Password = "%4fgT1_3ioR",
                    Port = 587
                });

            _harness = new InMemoryTestHarness();
            _consumerTestHarness = _harness.Consumer(() =>
                _mocker.CreateInstance<SendEmailConsumer>());
            
        }

        [Test]
        public async Task ShouldThrowExceptionWhenEmailTemplateWasNotFound()
        {
            _mocker
                .Setup<IEmailTemplateRepository, DbEmailTemplate>(x => x.GetEmailTemplateById(_dbEmailTemplate.Id))
                .Returns(new DbEmailTemplate());

            var language = "en";
            var senderId = Guid.NewGuid();
            var emailRecipient = "malkinevgeniy11@gmail.com";

            var expected = new
            {
                IsSuccess = false,
                Errors = new List<string> { "Email template text was not found." },
                Body = false
            };

            await _harness.Start();

            try
            {
                _requestClient = await _harness.ConnectRequestClient<ISendEmailRequest>();

                var response = await _requestClient.GetResponse<IOperationResult<bool>>(
                ISendEmailRequest.CreateObj(
                    _dbEmailTemplate.Id,
                    senderId,
                    emailRecipient,
                    language,
                    _templateTags));

                Assert.IsFalse(response.Message.IsSuccess);
                Assert.AreEqual(expected.Errors, response.Message.Errors);
                SerializerAssert.AreEqual(expected, response.Message);
                Assert.True(_consumerTestHarness.Consumed.Select<ISendEmailRequest>().Any());
                Assert.True(_harness.Sent.Select<IOperationResult<bool>>().Any());
                _mocker
                    .Verify<IEmailTemplateRepository, DbEmailTemplate>(x => x.GetEmailTemplateById(_dbEmailTemplate.Id), Times.Once);
            }
            finally
            {
                await _harness.Stop();
            }
        }

        [Test]
        public async Task ShouldSendEmail()
        {
            _mocker
                .Setup<IEmailTemplateRepository, DbEmailTemplate>(x => x.GetEmailTemplateById(_dbEmailTemplate.Id))
                .Returns(_dbEmailTemplate);

            var language = "en";
            var senderId = Guid.NewGuid();
            var emailRecipient = "malkinevgeniy11@gmail.com";

            var expected = new
            {
                IsSuccess = true,
                Errors = null as List<string>,
                Body = true
            };

            var dbEmail = new DbEmail
            {
                Id = Guid.NewGuid(),
                Receiver = emailRecipient,
                SenderId = senderId,
                Time = DateTime.UtcNow
            };

            await _harness.Start();

            try
            {
                _requestClient = await _harness.ConnectRequestClient<ISendEmailRequest>();

                var response = await _requestClient.GetResponse<IOperationResult<bool>>(
                ISendEmailRequest.CreateObj(
                    _dbEmailTemplate.Id,
                    senderId,
                    emailRecipient,
                    language,
                    _templateTags));

                Assert.True(response.Message.IsSuccess);
                Assert.AreEqual(null, response.Message.Errors);
                SerializerAssert.AreEqual(expected, response.Message);
                Assert.True(_consumerTestHarness.Consumed.Select<ISendEmailRequest>().Any());
                Assert.True(_harness.Sent.Select<IOperationResult<bool>>().Any());
                _mocker
                    .Verify<IEmailTemplateRepository, DbEmailTemplate>(x => x.GetEmailTemplateById(_dbEmailTemplate.Id), Times.Once);
            }
            finally
            {
                await _harness.Stop();
            }
        }
    }
}
