﻿using System;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Db;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.MessageService.Data.Interfaces
{
  [AutoInject]
  public interface IEmailTemplateTextRepository
  {
    Guid? Create(DbEmailTemplateText request);

    bool Edit(Guid emailTemplateTextId, JsonPatchDocument<DbEmailTemplateText> patch);
  }
}