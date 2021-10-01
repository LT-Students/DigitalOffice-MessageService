using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.EmailTemplate;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.MessageService.Mappers.Db.Interfaces
{
  [AutoInject]
  public interface IPatchDbEmailTemplateMapper
  {
    JsonPatchDocument<DbEmailTemplate> Map(
        JsonPatchDocument<EditEmailTemplateRequest> request);
  }
}
