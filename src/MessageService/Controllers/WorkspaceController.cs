using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Business.Commands.Workspace.Interfaces;
using LT.DigitalOffice.MessageService.Models.Dto.Models;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace.Filters;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.MessageService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WorkspaceController : ControllerBase
    {
        [HttpPost("create")]
        public OperationResultResponse<Guid> Create(
            [FromServices] ICreateWorkspaceCommand command,
            [FromBody] CreateWorkspaceRequest request)
        {
            return command.Execute(request);
        }

        [HttpGet("find")]
        public FindResultResponse<ShortWorkspaceInfo> Find(
            [FromServices] IFindWorkspaceCommand command,
            [FromQuery] FindWorkspaceFilter filter)
        {
            return command.Execute(filter);
        }

    [HttpGet("get")]
    public OperationResultResponse<WorkspaceInfo> Get(
            [FromServices] IGetWorkspaceCommand command,
            [FromQuery] GetWorkspaceFilter filter)
    {
      return command.Execute(filter);
    }

    [HttpDelete("remove")]
        public OperationResultResponse<bool> Remove(
            [FromServices] IRemoveWorkspaceCommand command,
            [FromQuery] Guid workspaceId)
        {
            return command.Execute(workspaceId);
        }
    }
}
