using LT.DigitalOffice.MessageService.Business.WorkspaceCommands.Interfaces;
using LT.DigitalOffice.MessageService.Models.Dto.Responses;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.MessageService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkspaceController : ControllerBase
    {
        [HttpPost("create")]
        public Guid Create(
            [FromServices] ICreateWorkspaceCommand command,
            [FromBody] Workspace request)
        {
            return command.Execute(request);
        }
    }
}
