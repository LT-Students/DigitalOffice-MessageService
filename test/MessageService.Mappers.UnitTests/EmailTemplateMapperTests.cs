using LT.DigitalOffice.Kernel.UnitTestLibrary;
using LT.DigitalOffice.MessageService.Mappers.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.MessageService.Mappers.UnitTests
{
    public class EmailTemplateMapperTests
    {
        private IMapper<EmailTemplate, DbEmailTemplate> mapper;

        private EmailTemplate emailTemplate;
        private DbEmailTemplate dbEmailTemplate;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            mapper = new EmailTemplateMapper();

            emailTemplate = new EmailTemplate
            {
                Subject = "Subject",
                Body = "Body",
                AuthorId = Guid.NewGuid()
            };

            dbEmailTemplate = new DbEmailTemplate
            {
                Subject = emailTemplate.Subject,
                Body = emailTemplate.Body,
                AuthorId = emailTemplate.AuthorId,
                IsActive = true
            };
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWhenEmailTemplateIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => mapper.Map(null));
        }

        [Test]
        public void ShouldReturnRightModelWhenEmailTemplateIsMapped()
        {
            var resultDbEmailTemplate = mapper.Map(emailTemplate);
            dbEmailTemplate.Id = resultDbEmailTemplate.Id;
            dbEmailTemplate.CreatedAt = resultDbEmailTemplate.CreatedAt;

            Assert.IsInstanceOf<Guid>(resultDbEmailTemplate.Id);
            SerializerAssert.AreEqual(dbEmailTemplate, resultDbEmailTemplate);
        }
    }
}