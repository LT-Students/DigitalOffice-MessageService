using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.EmailTemplateText;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.MessageService.Mappers.Db.Interfaces
{
  [AutoInject]
  public interface IPatchDbEmailTemplateTextMapper
  {
    JsonPatchDocument<DbEmailTemplateText> Map(
      JsonPatchDocument<EditEmailTemplateTextRequest> request);
  }
}
