using System.Linq;
using System.Net;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Business.Commands.Workspace.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Models.Interfaces;
using LT.DigitalOffice.MessageService.Models.Dto.Models;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace.Filters;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.MessageService.Business.Commands.Workspace
{
  public class FindWorkspaceCommand : IFindWorkspaceCommand
  {
    private readonly IWorkspaceRepository _repository;
    private readonly IShortWorkspaceInfoMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public FindWorkspaceCommand(
        IWorkspaceRepository repository,
        IShortWorkspaceInfoMapper mapper,
        IHttpContextAccessor httpContextAccessor)
    {
      _repository = repository;
      _mapper = mapper;
      _httpContextAccessor = httpContextAccessor;
    }

    public FindResultResponse<ShortWorkspaceInfo> Execute(FindWorkspaceFilter filter)
    {
      FindResultResponse<ShortWorkspaceInfo> response = new();

      response.Body = _repository
        .Find(filter, out int totalCount, response.Errors)
        .Select(_mapper.Map)
        .ToList();

      response.TotalCount = totalCount;
      response.Status = OperationResultStatusType.FullSuccess;

      if (response.Errors.Any())
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        response.Status = OperationResultStatusType.Failed;
      }

      return response;
    }
  }
}
