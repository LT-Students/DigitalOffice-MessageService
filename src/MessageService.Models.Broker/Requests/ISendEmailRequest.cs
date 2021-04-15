﻿using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Dto.Configurations;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.Broker.Requests
{
    [AutoInjectRequest(nameof(RabbitMqConfig.SendEmailEndpoint))]
    public interface ISendEmailRequest
    {
        Guid TemplateId { get; }
        Guid SenderId { get; }
        string Email { get; }
        string Language { get; }
        IDictionary<string, string> TemplateValues { get; }

        static object CreateObj(
            Guid templateId,
            Guid senderId,
            string email,
            string language,
            IDictionary<string, string>  templateValues)
        {
            return new
            {
                Email = email,
                Language = language,
                SenderId = senderId,
                TemplateId = templateId,
                TemplateValues = templateValues
            };
        }
    }
}
