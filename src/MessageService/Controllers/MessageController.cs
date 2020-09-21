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
        [HttpPost("addEmailTemplate")]
        public Guid AddEmailTemplate(
            [FromServices] IAddEmailTemplateCommand command,
            [FromBody] EmailTemplate emailTemplate,
            [FromHeader] Guid requestingUser)
        {
            return command.Execute(emailTemplate, requestingUser);
        }
    }
}
