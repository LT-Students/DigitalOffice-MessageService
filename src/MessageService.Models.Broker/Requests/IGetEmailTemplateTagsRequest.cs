using LT.DigitalOffice.MessageService.Models.Dto.Enums;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.Broker.Requests
{
    public interface IGetEmailTemplateTagsRequest
    {
        Guid? TemplateId { get; }
        string Language { get; }
        EmailTemplateType Type { get; }

        static object CreateObj(string language, EmailTemplateType type, Guid? templateId = null)
        {
            return new
            {
                TemplateId = templateId,
                Type = type,
                Language = language,
            };
        }
    }
}
