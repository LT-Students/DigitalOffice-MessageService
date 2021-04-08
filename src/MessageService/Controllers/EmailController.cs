using LT.DigitalOffice.MessageService.Business.EmailTemplatesCommands.Interfaces;
using LT.DigitalOffice.MessageService.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.MessageService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        [HttpGet("removeEmailTemplate")]
        public void RemoveEmailTemplate(
            [FromServices] IDisableEmailTemplateCommand command,
            [FromQuery] Guid emailTemplateId)
        {
            command.Execute(emailTemplateId);
        }

        [HttpPost("addEmailTemplate")]
        public Guid AddEmailTemplate(
            [FromServices] IAddEmailTemplateCommand command,
            [FromBody] EmailTemplate emailTemplate)
        {
            return command.Execute(emailTemplate);
        }

        [HttpPost("editEmailTemplate")]
        public void EditEmailtemplate(
            [FromServices] IEditEmailTemplateCommand command,
            [FromBody] EditEmailTemplateRequest emailTemplate)
        {
            command.Execute(emailTemplate);
        }
    }
}
