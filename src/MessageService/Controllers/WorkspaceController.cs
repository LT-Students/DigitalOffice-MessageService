using System;
using System.Net;
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
    private readonly IHttpContextAccessor _httpContextAccessor;

    public WorkspaceController(IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost("create")]
    public OperationResultResponse<Guid?> Create(
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
      var result = command.Execute(filter);

      if (result.Status == OperationResultStatusType.Failed)
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
      }

      return result;
    }

    [HttpPatch("edit")]
    public OperationResultResponse<bool> Edit(
            [FromServices] IEditWorkspaceCommand command,
            [FromQuery] Guid workspaceId,
            [FromBody] JsonPatchDocument<EditWorkspaceRequest> request)
    {
      return command.Execute(workspaceId, request);
    }
  }
}
