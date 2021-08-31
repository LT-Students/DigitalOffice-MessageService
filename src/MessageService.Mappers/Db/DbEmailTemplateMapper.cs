using System;
using System.Linq;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.MessageService.Mappers.Db.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.EmailTemplate;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.MessageService.Mappers.Db
{
  public class DbEmailTemplateMapper : IDbEmailTemplateMapper
  {
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DbEmailTemplateMapper(IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor;
    }

    public DbEmailTemplate Map(EmailTemplateRequest emailTemplate)
    {
      if (emailTemplate == null)
      {
        return null;
      }

      var templateId = Guid.NewGuid();

      return new DbEmailTemplate
      {
        Id = templateId,
        Name = emailTemplate.Name,
        Type = (int)emailTemplate.Type,
        IsActive = true,
        CreatedAtUtc = DateTime.UtcNow,
        CreatedBy = _httpContextAccessor.HttpContext.GetUserId(),
        EmailTemplateTexts = emailTemplate.EmailTemplateTexts.Select(x =>
          new DbEmailTemplateText
          {
            Id = Guid.NewGuid(),
            EmailTemplateId = templateId,
            Subject = x.Subject,
            Text = x.Text,
            Language = x.Language
          })
          .ToList()
      };
    }
  }
}
