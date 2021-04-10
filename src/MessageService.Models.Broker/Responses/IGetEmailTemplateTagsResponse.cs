using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.Broker.Responses
{
    public interface IGetEmailTemplateTagsResponse
    {
        IDictionary<string, string> TemplateTags { get; }

        static object CreateObj(IDictionary<string, string> templateTags)
        {
            return new
            {
                TemplateTags = templateTags
            };
        }

        public IDictionary<string, string> CreateDictionaryTemplate(string userFirstName, string userEmail, string userId, string link)
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

            if (isKey(link))
            {
                TemplateTags[nameof(link)] = link;
            }

            return TemplateTags;
        }
    }
}
