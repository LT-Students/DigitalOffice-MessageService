using System;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Business.Commands.EmailTemplate.Interfaces;
using LT.DigitalOffice.MessageService.Models.Dto.Models.Emails;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.EmailTemplate;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.MessageService.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class EmailTemplateController : ControllerBase
  {
    [HttpPost("create")]
    public OperationResultResponse<Guid?> Create(
      [FromServices] ICreateEmailTemplateCommand command,
      [FromBody] EmailTemplateRequest request)
    {
      return command.Execute(request);
    }

    [HttpPost("edit")]
    public OperationResultResponse<bool> Edit(
      [FromServices] IEditEmailTemplateCommand command,
      [FromQuery] Guid emailTemplateId,
      [FromBody] JsonPatchDocument<EditEmailTemplateRequest> patch)
    {
      return command.Execute(emailTemplateId, patch);
    }

    [HttpGet("find")]
    public FindResultResponse<EmailTemplateInfo> Find(
      [FromServices] IFindEmailTemplateCommand command,
      [FromQuery] int skipCount,
      [FromQuery] int takeCount,
      [FromQuery] bool includeDeactivated = false)
    {
      return command.Execute(skipCount, takeCount, includeDeactivated);
    }
  }
}
