using LT.DigitalOffice.MessageService.Mappers.EmailMappers;
using LT.DigitalOffice.MessageService.Mappers.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto;
using LT.DigitalOffice.UnitTestKernel;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.MessageService.Mappers.UnitTests
{
    public class EmailTemplateMapperTests
    {
        private IMapper<EmailTemplate, DbEmailTemplate> mapper;
        private IMapper<EditEmailTemplateRequest, DbEmailTemplate> editMapper;

        private EmailTemplate emailTemplate;
        private DbEmailTemplate dbEmailTemplate;
        private EditEmailTemplateRequest editEmailTemplateRequest;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            mapper = new EmailTemplateMapper();
            editMapper = new EmailTemplateMapper();

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

            editEmailTemplateRequest = new EditEmailTemplateRequest
            {
                Id = Guid.NewGuid(),
                Subject = "Subject_1",
                Body = "Body_1"
            };
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWhenEmailTemplateIsNull()
        {
            emailTemplate = null;

            Assert.Throws<ArgumentNullException>(() => mapper.Map(emailTemplate));
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

        [Test]
        public void ShouldThrowArgumentNullExceptionWhenEditEmailTemplateRequestIsNull()
        {
            EditEmailTemplateRequest newEditEmailTemplateRequest = null;

            Assert.Throws<ArgumentNullException>(() => editMapper.Map(newEditEmailTemplateRequest));
        }

        [Test]
        public void ShouldReturnRightModelSuccessful()
        {
            var expectedDbEmailTemplate = new DbEmailTemplate
            {
                Subject = editEmailTemplateRequest.Subject,
                Body = editEmailTemplateRequest.Body,
            };

            SerializerAssert.AreEqual(expectedDbEmailTemplate, editMapper.Map(editEmailTemplateRequest));
        }
    }
}