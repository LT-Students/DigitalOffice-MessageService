using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.Broker.Responses
{
    public interface IGetEmailTemplateTagsResponse
    {
        Guid TemplateId { get; }
        IDictionary<string, string> TemplateTags { get; }

        static object CreateObj(IDictionary<string, string> templateTags, Guid templateId)
        {
            return new
            {
                TemplateId = templateId,
                TemplateTags = templateTags
            };
        }

        public IDictionary<string, string> CreateDictionaryTemplate(
            string userFirstName,
            string userEmail,
            string userId,
            string userPassword,
            string secret)
        {
            Func<string, bool> isKey = arg => !string.IsNullOrEmpty(arg) && TemplateTags.ContainsKey(nameof(arg));

            if (isKey(userFirstName))
            {
                TemplateTags[nameof(userFirstName)] = userFirstName;
            }

            if (isKey(userEmail))
            {
                TemplateTags[nameof(userEmail)] = userEmail;
            }

            if (isKey(userId))
            {
                TemplateTags[nameof(userId)] = userId;
            }

            if (isKey(userPassword))
            {
                TemplateTags[nameof(userPassword)] = userPassword;
            }

            if (isKey(secret))
            {
                TemplateTags[nameof(secret)] = secret;
            }

            return TemplateTags;
        }
    }
}
