using System;
using System.Linq;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Data.Provider;
using LT.DigitalOffice.MessageService.Models.Db;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.MessageService.Data
{
  public class EmailTemplateTextRepository : IEmailTemplateTextRepository
  {
    private readonly IDataProvider _provider;

    public EmailTemplateTextRepository(IDataProvider provider)
    {
      _provider = provider;
    }

    public Guid? Create(DbEmailTemplateText request)
    {
      if (request == null)
      {
        return null;
      }

      _provider.EmailTemplateTexts.Add(request);
      _provider.Save();

      return request.Id;
    }

    public bool Edit(Guid emailTemplateTextId, JsonPatchDocument<DbEmailTemplateText> patch)
    {
      if (patch == null)
      {
        return false;
      }

      DbEmailTemplateText dbEmailTemplateText =
        _provider
        .EmailTemplateTexts
        .FirstOrDefault(et => et.Id == emailTemplateTextId);

      if (dbEmailTemplateText == null)
      {
        return false;
      }

      patch.ApplyTo(dbEmailTemplateText);
      _provider.Save();

      return true;
    }
  }
}
