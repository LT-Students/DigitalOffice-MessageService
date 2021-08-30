using System;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Business.Commands.EmailTemplateText.Interfaces;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.EmailTemplate;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.EmailTemplateText;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.MessageService.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class EmailTemplateTextController : ControllerBase
  {
    [HttpPost("create")]
    public OperationResultResponse<Guid?> Create(
      [FromServices] ICreateEmailTemplateTextCommand command,
      [FromBody] EmailTemplateTextRequest request)
    {
      return command.Execute(request);
    }

    [HttpPost("edit")]
    public OperationResultResponse<bool> Edit(
      [FromServices] IEditEmailTemplateTextCommand command,
      [FromQuery] Guid emailTemplateTextId,
      [FromBody] JsonPatchDocument<EditEmailTemplateTextRequest> patch)
    {
      return command.Execute(emailTemplateTextId, patch);
    }
  }
}
