using System;
using System.Net;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Business.Commands.Workspace.Interfaces;
using LT.DigitalOffice.MessageService.Models.Dto.Models;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.MessageService.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class WorkspaceController : ControllerBase
  {
    [HttpPost("create")]
    public async Task<OperationResultResponse<Guid?>> CreateAsync(
      [FromServices] ICreateWorkspaceCommand command,
      [FromBody] CreateWorkspaceRequest request)
    {
      return await command.ExecuteAsync(request);
    }

    [HttpGet("find")]
    public async Task<FindResultResponse<ShortWorkspaceInfo>> FindAsync(
      [FromServices] IFindWorkspaceCommand command,
      [FromQuery] FindWorkspaceFilter filter)
    {
      return await command.ExecuteAsync(filter);
    }

    [HttpGet("get")]
    public async Task<OperationResultResponse<WorkspaceInfo>> GetAsync(
      [FromServices] IGetWorkspaceCommand command,
      [FromQuery] GetWorkspaceFilter filter)
    {
      return await command.ExecuteAsync(filter);
    }

    [HttpPatch("edit")]
    public async Task<OperationResultResponse<bool>> EditAsync(
      [FromServices] IEditWorkspaceCommand command,
      [FromQuery] Guid workspaceId,
      [FromBody] JsonPatchDocument<EditWorkspaceRequest> request)
    {
      return await command.ExecuteAsync(workspaceId, request);
    }
  }
}
