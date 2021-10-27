using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Business.Commands.Workspace.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Patch.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace;
using LT.DigitalOffice.MessageService.Validation.Validators.Workspace.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.MessageService.Business.Commands.Workspace
{
  public class EditWorkspaceCommand : IEditWorkspaceCommand
  {
    private readonly IEditWorkspaceRequestValidator _validator;
    private readonly IPatchDbWorkspaceMapper _mapper;
    private readonly IWorkspaceRepository _repository;
    private readonly IWorkspaceUserRepository _userRepository;
    private readonly IAccessValidator _accessValidator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public EditWorkspaceCommand(
      IEditWorkspaceRequestValidator validator,
      IPatchDbWorkspaceMapper mapper,
      IWorkspaceRepository repository,
      IWorkspaceUserRepository userRepository,
      IAccessValidator accessValidator,
      IHttpContextAccessor httpContextAccessor)
    {
      _validator = validator;
      _mapper = mapper;
      _repository = repository;
      _userRepository = userRepository;
      _accessValidator = accessValidator;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task<OperationResultResponse<bool>> ExecuteAsync(Guid workspaceId, JsonPatchDocument<EditWorkspaceRequest> request)
    {
      DbWorkspace dbWorkspace = _repository.Get(workspaceId);

      Guid editorId = _httpContextAccessor.HttpContext.GetUserId();

      if (dbWorkspace.CreatedBy != editorId
        && _userRepository.GetAdmins(workspaceId).FirstOrDefault(wa => wa.UserId == editorId) == null
        && !await _accessValidator.IsAdminAsync())
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;

        return new OperationResultResponse<bool>
        {
          Status = OperationResultStatusType.Failed,
          Errors = new() { "Not enough rights." }
        };
      }

      if (!_validator.ValidateCustom(request, out List<string> errors))
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        return new OperationResultResponse<bool>
        {
          Status = OperationResultStatusType.Failed,
          Errors = errors
        };
      }

      OperationResultResponse<bool> response = new();

      response.Body = _repository.Edit(dbWorkspace, _mapper.Map(request), editorId);

      if (!response.Body)
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        response.Errors.Add("Bad request");
        response.Status = OperationResultStatusType.Failed;
        return response;
      }

      response.Status = OperationResultStatusType.FullSuccess;
      return response;
    }
  }
}
