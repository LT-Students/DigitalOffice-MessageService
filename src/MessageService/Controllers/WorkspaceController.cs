using System;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Business.Commands.Workspace.Interfaces;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace.Filters;
using LT.DigitalOffice.MessageService.Models.Dto.Responses.Workspace;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.MessageService.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class WorkspaceController : ControllerBase
  {
    [HttpPost]
    public async Task<OperationResultResponse<Guid?>> CreateAsync(
      [FromServices] ICreateWorkspaceCommand command,
      [FromBody] CreateWorkspaceRequest request)
    {
      return await command.ExecuteAsync(request);
    }

    [HttpGet]
    public async Task<FindResultResponse<ShortWorkspaceInfo>> FindAsync(
      [FromServices] IFindWorkspaceCommand command,
      [FromQuery] FindWorkspaceFilter filter)
    {
      return await command.ExecuteAsync(filter);
    }

    [HttpGet("{workspaceId}")]
    public async Task<OperationResultResponse<WorkspaceInfo>> GetAsync(
      [FromServices] IGetWorkspaceCommand command,
      [FromRoute] Guid workspaceId,
      [FromQuery] GetWorkspaceFilter filter)
    {
      return await command.ExecuteAsync(workspaceId, filter);
    }

    [HttpPatch("{workspaceId}")]
    public async Task<OperationResultResponse<bool>> EditAsync(
      [FromServices] IEditWorkspaceCommand command,
      [FromRoute] Guid workspaceId,
      [FromBody] JsonPatchDocument<EditWorkspaceRequest> request)
    {
      return await command.ExecuteAsync(workspaceId, request);
    }
  }
}
