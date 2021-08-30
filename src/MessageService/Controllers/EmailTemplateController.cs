using System;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Business.Commands.EmailTemplate.Interfaces;
using LT.DigitalOffice.MessageService.Models.Dto.Models.Emails;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.EmailTemplate;
using LT.DigitalOffice.MessageService.Models.Dto.Responses;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.MessageService.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class EmailTemplateController : ControllerBase
  {
    [HttpDelete("remove")]
    public OperationResultResponse<bool> Remove(
        [FromServices] IDisableEmailTemplateCommand command,
        [FromQuery] Guid emailTemplateId)
    {
      return command.Execute(emailTemplateId);
    }

    [HttpPost("create")]
    public OperationResultResponse<Guid> Create(
        [FromServices] ICreateEmailTemplateCommand command,
        [FromBody] EmailTemplateRequest request)
    {
      return command.Execute(request);
    }

    [HttpPost("edit")]
    public OperationResultResponse<bool> Edit(
        [FromServices] IEditEmailTemplateCommand command,
        [FromBody] EditEmailTemplateRequest emailTemplate)
    {
      return command.Execute(emailTemplate);
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
