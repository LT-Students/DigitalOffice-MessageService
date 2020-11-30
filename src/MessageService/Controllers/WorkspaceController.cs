using LT.DigitalOffice.MessageService.Business.Interfaces;
using LT.DigitalOffice.MessageService.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.MessageService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkspaceController : ControllerBase
    {
        [HttpPost("addWorkspace")]
        public Guid AddWorkspace(
            [FromServices] IAddWorkspaceCommand command,
            [FromBody] AddWorkspaceRequest request)
        {
            return command.Execute(request);
        }
    }
}
