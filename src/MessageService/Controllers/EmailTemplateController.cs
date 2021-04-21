using LT.DigitalOffice.MessageService.Business.EmailTemplatesCommands.Interfaces;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.EmailTemplate;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.MessageService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EmailTemplateController : ControllerBase
    {
        [HttpGet("remove")]
        public void Remove(
            [FromServices] IDisableEmailTemplateCommand command,
            [FromQuery] Guid emailTemplateId)
        {
            command.Execute(emailTemplateId);
        }

        [HttpPost("create")]
        public Guid Create(
            [FromServices] ICreateEmailTemplateCommand command,
            [FromBody] EmailTemplateRequest emailTemplate)
        {
            return command.Execute(emailTemplate);
        }

        [HttpPost("edit")]
        public void Edit(
            [FromServices] IEditEmailTemplateCommand command,
            [FromBody] EditEmailTemplateRequest emailTemplate)
        {
            command.Execute(emailTemplate);
        }
    }
}
