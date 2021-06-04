using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.MessageService.Broker.Consumers;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Enums;
using LT.DigitalOffice.Models.Broker.Enums;
using LT.DigitalOffice.Models.Broker.Requests.Message;
using LT.DigitalOffice.Models.Broker.Responses.Message;
using LT.DigitalOffice.UnitTestKernel;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LT.DigitalOffice.MessageService.Broker.UnitTests
{
    class EmailTemplateTagsConsumerTests
    {
        private IDictionary<string, string> _templateTags;

        private DbEmailTemplate _dbEmailTemplate;
        private InMemoryTestHarness _harness;
        private Mock<ILogger<EmailTemplateTagsConsumer>> _logger;
        private Mock<IEmailTemplateRepository> _templateRepositoryMock;
        private IRequestClient<IGetEmailTemplateTagsRequest> _requestClient;
        private ConsumerTestHarness<EmailTemplateTagsConsumer> _consumerTestHarness;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _harness = new InMemoryTestHarness();
            _logger = new Mock<ILogger<EmailTemplateTagsConsumer>>();

            _templateTags = new Dictionary<string, string>();

            _templateTags.Add("userFirstName", "");
            _templateTags.Add("userPassword", "");

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

            dbEmailTemplateText.EmailTemplate = _dbEmailTemplate;
        }

        [SetUp]
        public void SetUp()
        {
            _templateRepositoryMock = new Mock<IEmailTemplateRepository>();
            _harness = new InMemoryTestHarness();
            _consumerTestHarness = _harness.Consumer(() =>
                new EmailTemplateTagsConsumer(
                    _logger.Object,
                    _templateRepositoryMock.Object));
        }

        [Test]
        public async Task ShouldThrowExceptionWhenEmailTemplateWasNotFound()
        {
            _templateRepositoryMock
                .Setup(x => x.GetEmailTemplateByType((int)EmailTemplateType.Greeting))
                .Returns(new DbEmailTemplate());

            var language = "en";

            var expected = new
            {
                IsSuccess = false,
                Errors = new List<string> { "Email template text was not found." }
            };

            await _harness.Start();

            try
            {
                _requestClient = await _harness.ConnectRequestClient<IGetEmailTemplateTagsRequest>();

                var response = await _requestClient.GetResponse<IOperationResult<IGetEmailTemplateTagsResponse>>(
                IGetEmailTemplateTagsRequest.CreateObj(
                    language,
                    EmailTemplateType.Greeting));

                Assert.IsFalse(response.Message.IsSuccess);
                Assert.AreEqual(expected.Errors, response.Message.Errors);
                Assert.True(_consumerTestHarness.Consumed.Select<IGetEmailTemplateTagsRequest>().Any());
                Assert.True(_harness.Sent.Select<IOperationResult<IGetEmailTemplateTagsResponse>>().Any());
                _templateRepositoryMock.Verify(x => x.GetEmailTemplateByType((int)EmailTemplateType.Greeting), Times.Once);
            }
            finally
            {
                await _harness.Stop();
            }
        }

        [Test]
        public async Task ShouldGetTemplateTags()
        {
            _templateRepositoryMock
                .Setup(x => x.GetEmailTemplateByType((int)EmailTemplateType.Greeting))
                .Returns(_dbEmailTemplate);

            var language = "en";

            var expected = new
            {
                IsSuccess = true,
                Errors = null as List<string>,
                Body = new
                {
                    TemplateId = _dbEmailTemplate.Id,
                    TemplateTags = _templateTags
                }
            };

            await _harness.Start();

            try
            {
                _requestClient = await _harness.ConnectRequestClient<IGetEmailTemplateTagsRequest>();

                var response = await _requestClient.GetResponse<IOperationResult<IGetEmailTemplateTagsResponse>>(
                IGetEmailTemplateTagsRequest.CreateObj(
                    language,
                    EmailTemplateType.Greeting));

                Assert.True(response.Message.IsSuccess);
                Assert.AreEqual(null, response.Message.Errors);
                SerializerAssert.AreEqual(expected, response.Message);
                Assert.True(_consumerTestHarness.Consumed.Select<IGetEmailTemplateTagsRequest>().Any());
                Assert.True(_harness.Sent.Select<IOperationResult<IGetEmailTemplateTagsResponse>>().Any());
                _templateRepositoryMock.Verify(x => x.GetEmailTemplateByType((int)EmailTemplateType.Greeting), Times.Once);
            }
            finally
            {
                await _harness.Stop();
            }
        }
    }
}
