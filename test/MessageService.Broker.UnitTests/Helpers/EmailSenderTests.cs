using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.UserService.Business.Helpers.Email;
using Microsoft.Extensions.Options;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.MessageService.Broker.UnitTests.Helpers
{
    public class EmailSenderTests
    {
        private EmailSender _sender;
        private AutoMocker _mocker;

        [SetUp]
        public void SetUp()
        {
            _mocker = new AutoMocker();
            _sender = _mocker.CreateInstance<EmailSender>();

            _mocker
                .Setup<IOptions<SmtpCredentialsOptions>, SmtpCredentialsOptions>(x => x.Value)
                .Returns(new SmtpCredentialsOptions
                {
                    Email = "lt.digitaloffice@gmail.com",
                    Host = "smtp.gmail.com",
                    Password = "%4fgT1_3ioR",
                    Port = 587
                });
        }

        [Test]
        public void ShouldResendEmailSuccessful()
        {
            var id = Guid.NewGuid();

            _mocker
                .Setup<IUnsentEmailRepository, DbUnsentEmail>(x => x.Get(It.IsAny<Guid>()))
                .Returns(new DbUnsentEmail
                    {
                        Id = Guid.NewGuid(),
                        Email = new DbEmail
                        {
                            Id = id,
                            Body = "Body",
                            Receiver = "belehov.egor2001@yandex.ru",
                            Subject = "Subject",
                            Time = DateTime.UtcNow
                        },
                        CreatedAt = DateTime.UtcNow,
                        LastSendAt = DateTime.UtcNow,
                        EmailId = id,
                        TotalSendingCount = 1
                });

            _mocker
                .Setup<IUnsentEmailRepository, bool>(x => x.Remove(It.IsAny<DbUnsentEmail>()));

            Assert.IsTrue(_sender.ResendEmail(Guid.NewGuid()));
        }

        [Test]
        public void ShouldSendEmailSuccessful()
        {
            _mocker
                .Setup<IUnsentEmailRepository, bool>(x => x.Remove(It.IsAny<DbUnsentEmail>()));

            Assert.IsTrue(_sender.SendEmail("belehov.egor2001@yandex.ru", "afwgbalwgfp;aogwpga", "awgawgagaawihawkgfap;iwhfpafawf;alowhawnfl"));
        }
    }
}
