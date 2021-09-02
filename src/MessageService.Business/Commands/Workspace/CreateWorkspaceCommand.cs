using System;
using System.Collections.Generic;
using System.Net;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Business.Commands.Workspace.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Db.Workspace.Interfaces;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace;
using LT.DigitalOffice.MessageService.Validation.Workspace.Interfaces;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.MessageService.Business.Commands.Workspace
{
  public class CreateWorkspaceCommand : ICreateWorkspaceCommand
  {
    private readonly ICreateWorkspaceValidator _validator;
    private readonly IDbWorkspaceMapper _mapper;
    private readonly IWorkspaceRepository _repository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateWorkspaceCommand(
      ICreateWorkspaceValidator validator,
      IDbWorkspaceMapper mapper,
      IWorkspaceRepository repository,
      IHttpContextAccessor httpContextAccessor)
    {
      _validator = validator;
      _mapper = mapper;
      _repository = repository;
      _httpContextAccessor = httpContextAccessor;
    }

    public OperationResultResponse<Guid?> Execute(CreateWorkspaceRequest request)
    {
      if (!_validator.ValidateCustom(request, out List<string> errors))
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        return new OperationResultResponse<Guid?>
        {
          Status = OperationResultStatusType.Failed,
          Errors = errors
        };
      }

      OperationResultResponse<Guid?> response = new();

      response.Body = _repository.Add(_mapper.Map(request));

      if (response.Body == null)
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        response.Status = OperationResultStatusType.Failed;
        return response;
      }

      _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;

      response.Status = OperationResultStatusType.FullSuccess;

      return response;
    }
  }
}
