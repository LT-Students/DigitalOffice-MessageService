using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.Broker.Responses;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Exceptions;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LT.DigitalOffice.MessageService.Broker.Consumers
{
    public class EmailTemplateTagsConsumer : IConsumer<IGetEmailTemplateTagsRequest>
    {
        private readonly ILogger<EmailTemplateTagsConsumer> _logger;
        private readonly IEmailTemplateRepository _templateRepository;

        private DbEmailTemplateText GetDbEmailTemplateText(IGetEmailTemplateTagsRequest request)
        {
            DbEmailTemplate dbEmailTemplate;
            if (request.TemplateId != null)
            {
                dbEmailTemplate = _templateRepository.GetEmailTemplateById((Guid)request.TemplateId);
            }
            else
            {
                dbEmailTemplate = _templateRepository.GetEmailTemplateByType((int)request.Type);
            }

            DbEmailTemplateText dbEmailTemplateText;
            dbEmailTemplateText = dbEmailTemplate?.EmailTemplateTexts.FirstOrDefault();

            if (dbEmailTemplateText == null)
            {
                string messageTemp = "Email template text was not found.";
                _logger.LogWarning(messageTemp);

                throw new NotFoundException(messageTemp);
            }

            return dbEmailTemplateText;
        }

        public EmailTemplateTagsConsumer(
            ILogger<EmailTemplateTagsConsumer> logger,
            IEmailTemplateRepository templateRepository)
        {
            _logger = logger;
            _templateRepository = templateRepository;
        }

        public async Task Consume(ConsumeContext<IGetEmailTemplateTagsRequest> context)
        {
            var response = OperationResultWrapper.CreateResponse(GetEmailTemplateTags, context.Message);

            await context.RespondAsync<IOperationResult<IGetEmailTemplateTagsResponse>>(response);
        }

        public object GetEmailTemplateTags(IGetEmailTemplateTagsRequest request)
        {
            var dbEmailTemplateText = GetDbEmailTemplateText(request);

            Dictionary<string, string> templateTags = new();
            foreach (Match match in Regex.Matches(dbEmailTemplateText.Text, "{.*?}"))
            {
                var tag = match.Value.Trim('{', '}');
                templateTags.Add(tag, "");
            }

            return IGetEmailTemplateTagsResponse.CreateObj(templateTags, dbEmailTemplateText.EmailTemplate.Id);
        }
    }
}
