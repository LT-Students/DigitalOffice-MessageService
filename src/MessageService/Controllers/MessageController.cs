using LT.DigitalOffice.MessageService.Business.Interfaces;
using LT.DigitalOffice.MessageService.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.MessageService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        [HttpGet("removeEmailTemplate")]
        public void removeEmailTemplate(
            [FromServices] IDisableEmailTemplateCommand command,
            [FromQuery] Guid emailTemplateId,
            [FromHeader] Guid requestingUser)
        {
            command.Execute(emailTemplateId, requestingUser);
        }
        
        [HttpPost("addEmailTemplate")]
        public Guid AddEmailTemplate(
            [FromServices] IAddEmailTemplateCommand command,
            [FromBody] EmailTemplate emailTemplate,
            [FromHeader] Guid requestingUser)
        {
            return command.Execute(emailTemplate, requestingUser);
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
