using LT.DigitalOffice.MessageService.Business.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto;
using Moq;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.MessageService.Business.UnitTests
{
    public class AddEmailTemplateCommandTests
    {
        private Mock<IEmailTemplateRepository> repositoryMock;
        private IAddEmailTemplateCommand command;
        private Mock<IMapper<EmailTemplate, DbEmailTemplate>> mapperMock;

        private Guid emailId = Guid.NewGuid();
        private EmailTemplate emailTemplate = new EmailTemplate
        {

        };

        [SetUp]
        public void SetUp()
        {
            repositoryMock = new Mock<IEmailTemplateRepository>();
            mapperMock = new Mock<IMapper<EmailTemplate, DbEmailTemplate>>();

            command = new AddEmailTemplateCommand(mapperMock.Object, repositoryMock.Object);
        }

        [Test]
        public void ShouldAddEmailTemplateCorrectly()
        {
            repositoryMock
                .Setup(x => x.AddEmailTemplate(It.IsAny<DbEmailTemplate>()))
                .Returns(emailId);

            mapperMock
                .Setup(mapper => mapper.Map(It.IsAny<EmailTemplate>()))
                .Returns(new DbEmailTemplate());

            Assert.That(command.Execute(emailTemplate), Is.EqualTo(emailId));

            mapperMock.Verify();

            repositoryMock.Verify();
        }

        [Test]
        public void ShouldThrowExceptionWhenMapperThrowsIt()
        {
            mapperMock
                .Setup(mapper => mapper.Map(It.IsAny<EmailTemplate>()))
                .Throws<Exception>();

            Assert.Throws<Exception>(() => command.Execute(emailTemplate));

            mapperMock.Verify();

            repositoryMock.Verify(repository => repository.AddEmailTemplate(It.IsAny<DbEmailTemplate>()), Times.Never());
        }
    }
}