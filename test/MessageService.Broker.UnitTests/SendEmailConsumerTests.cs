using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.MessageService.Broker.Consumers;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Db.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.Models.Broker.Requests.Message;
using LT.DigitalOffice.UnitTestKernel;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
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
        private Mock<IOptions<SmtpCredentialsOptions>> _smtpCredentialsOptions;
        private Mock<ILogger<SendEmailConsumer>> _logger;
        private Mock<IEmailRepository> _emailRepositoryMock;
        private Mock<IEmailTemplateRepository> _templateRepositoryMock;
        private Mock<IDbEmailMapper> _mapperMock;
        private IRequestClient<ISendEmailRequest> _requestClient;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _logger = new Mock<ILogger<SendEmailConsumer>>();
            _smtpCredentialsOptions = new Mock<IOptions<SmtpCredentialsOptions>>();
            _mapperMock = new Mock<IDbEmailMapper>();

            _emailRepositoryMock = new Mock<IEmailRepository>();

            var smtpOptions = new SmtpCredentialsOptions();
            smtpOptions.Email = "lt.digitaloffice@gmail.com";
            smtpOptions.Host = "smtp.gmail.com";
            smtpOptions.Port = 587;
            smtpOptions.Password = "%4fgT1_3ioR";

            _smtpCredentialsOptions
                .SetupGet(x => x.Value)
                .Returns(smtpOptions);

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
            _templateRepositoryMock = new Mock<IEmailTemplateRepository>();
            _harness = new InMemoryTestHarness();
            _consumerTestHarness = _harness.Consumer(() =>
                new SendEmailConsumer(
                    _mapperMock.Object,
                    _emailRepositoryMock.Object,
                    _logger.Object,
                    _smtpCredentialsOptions.Object,
                    _templateRepositoryMock.Object));
        }

        [Test]
        public async Task ShouldThrowExceptionWhenEmailTemplateWasNotFound()
        {
            _templateRepositoryMock
                .Setup(x => x.GetEmailTemplateById(_dbEmailTemplate.Id))
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
                _templateRepositoryMock.Verify(x => x.GetEmailTemplateById(_dbEmailTemplate.Id), Times.Once);
            }
            finally
            {
                await _harness.Stop();
            }
        }

        //[Test]
        //public async Task ShouldSendEmail()
        //{
        //    _templateRepositoryMock
        //        .Setup(x => x.GetEmailTemplateById(_dbEmailTemplate.Id))
        //        .Returns(_dbEmailTemplate);

        //    var language = "en";
        //    var senderId = Guid.NewGuid();
        //    var emailRecipient = "malkinevgeniy11@gmail.com";

        //    var expected = new
        //    {
        //        IsSuccess = true,
        //        Errors = null as List<string>,
        //        Body = true
        //    };

        //    var dbEmail = new DbEmail
        //    {
        //        Id = Guid.NewGuid(),
        //        Receiver = emailRecipient,
        //        SenderId = senderId,
        //        Time = DateTime.UtcNow
        //    };

        //    _mapperMock
        //        .Setup(X => X.Map(It.IsAny<ISendEmailRequest>()))
        //        .Returns(dbEmail);

        //    await _harness.Start();

        //    try
        //    {
        //        _requestClient = await _harness.ConnectRequestClient<ISendEmailRequest>();

        //        var response = await _requestClient.GetResponse<IOperationResult<bool>>(
        //        ISendEmailRequest.CreateObj(
        //            _dbEmailTemplate.Id,
        //            senderId,
        //            emailRecipient,
        //            language,
        //            _templateTags));

        //        Assert.True(response.Message.IsSuccess);
        //        Assert.AreEqual(null, response.Message.Errors);
        //        SerializerAssert.AreEqual(expected, response.Message);
        //        Assert.True(_consumerTestHarness.Consumed.Select<ISendEmailRequest>().Any());
        //        Assert.True(_harness.Sent.Select<IOperationResult<bool>>().Any());
        //        _templateRepositoryMock.Verify(x => x.GetEmailTemplateById(_dbEmailTemplate.Id), Times.Once);
        //    }
        //    finally
        //    {
        //        await _harness.Stop();
        //    }
        //}
    }
}
