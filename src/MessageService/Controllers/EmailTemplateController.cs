using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Business.Commands.EmailTemplate.Interfaces;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.EmailTemplate;
using Microsoft.AspNetCore.Mvc;
using System;

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
            [FromBody] EmailTemplateRequest emailTemplate)
        {
            return command.Execute(emailTemplate);
        }

        [HttpPost("edit")]
        public OperationResultResponse<bool> Edit(
            [FromServices] IEditEmailTemplateCommand command,
            [FromBody] EditEmailTemplateRequest emailTemplate)
        {
            return command.Execute(emailTemplate);
        }
    }
}
