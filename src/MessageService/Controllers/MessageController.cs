using LT.DigitalOffice.MessageService.Business.Interfaces;
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
            [FromServices] IRemoveEmailTemplateCommand command,
            [FromQuery] Guid emailTemplateId,
            [FromHeader] Guid requestingUser)
        {
            command.Execute(emailTemplateId, requestingUser);
        }
    }
}
