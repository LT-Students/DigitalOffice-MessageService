using System;
using System.Collections.Generic;
using System.Linq;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Data.Provider;
using LT.DigitalOffice.MessageService.Models.Db;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.MessageService.Data
{
  public class EmailTemplateRepository : IEmailTemplateRepository
  {
    private readonly IDataProvider _provider;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public EmailTemplateRepository(
      IDataProvider provider,
      IHttpContextAccessor httpContextAccessor)
    {
      _provider = provider;
      _httpContextAccessor = httpContextAccessor;
    }

    public Guid? Create(DbEmailTemplate request)
    {
      if (request == null)
      {
        return null;
      }

      _provider.EmailTemplates.Add(request);
      _provider.Save();

      return request.Id;
    }

    public bool Edit(Guid emailTemplateId, JsonPatchDocument<DbEmailTemplate> patch)
    {
      if (patch == null)
      {
        return false;
      }

      DbEmailTemplate dbEmailTemplate = _provider.EmailTemplates
        .FirstOrDefault(et => et.Id == emailTemplateId);

      if (dbEmailTemplate == null)
      {
        return false;
      }

      patch.ApplyTo(dbEmailTemplate);
      dbEmailTemplate.ModifiedBy = _httpContextAccessor.HttpContext.GetUserId();
      dbEmailTemplate.ModifiedAtUtc = DateTime.UtcNow;
      _provider.Save();

      return true;
    }

    public DbEmailTemplate Get(Guid emailTemplateId)
    {
      return _provider.EmailTemplates
        .Include(et => et.EmailTemplateTexts)
        .FirstOrDefault(et => et.Id == emailTemplateId);
    }

    public DbEmailTemplate Get(int type)
    {
      return _provider.EmailTemplates
        .Include(et => et.EmailTemplateTexts)
        .FirstOrDefault(et => et.Type == type && et.IsActive);
    }

    public List<DbEmailTemplate> Find(int skipCount, int takeCount, out int totalCount, List<string> errors, bool includeDeactivated)
    {
      if (skipCount < 0)
      {
        errors.Add("Skip count can't be less than 0.");
        totalCount = 0;
        return null;
      }

      if (takeCount < 1)
      {
        errors.Add("Take count can't be equal or less than 0.");
        totalCount = 0;
        return null;
      }

      IQueryable<DbEmailTemplate> dbEmailTemplates = _provider.EmailTemplates.AsQueryable();

      if (includeDeactivated)
      {
        totalCount = _provider.EmailTemplates.Count();
      }
      else
      {
        dbEmailTemplates = dbEmailTemplates.Where(e => e.IsActive);
        totalCount = _provider.EmailTemplates.Count(e => e.IsActive);
      }

      return dbEmailTemplates.Skip(skipCount).Take(takeCount).Include(e => e.EmailTemplateTexts).ToList();
    }
  }
}
