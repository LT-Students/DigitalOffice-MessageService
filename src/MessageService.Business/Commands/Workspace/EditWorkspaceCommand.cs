using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
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
    private readonly IResponseCreater _responseCreator;

    public EditWorkspaceCommand(
      IEditWorkspaceRequestValidator validator,
      IPatchDbWorkspaceMapper mapper,
      IWorkspaceRepository repository,
      IWorkspaceUserRepository userRepository,
      IAccessValidator accessValidator,
      IHttpContextAccessor httpContextAccessor,
      IResponseCreater responseCreator)
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
      DbWorkspace dbWorkspace = await _repository.GetAsync(workspaceId);

      Guid editorId = _httpContextAccessor.HttpContext.GetUserId();

      if (dbWorkspace.CreatedBy != editorId
        && (await _userRepository.GetAdminsAsync(workspaceId)).FirstOrDefault(wa => wa.UserId == editorId) == null
        && !await _accessValidator.IsAdminAsync())
      {
        return _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.Forbidden);
      }

      if (!_validator.ValidateCustom(request, out List<string> errors))
      {
        return _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.BadRequest, errors);
      }

      OperationResultResponse<bool> response = new();

      response.Body = await _repository.EditAsync(dbWorkspace, await _mapper.MapAsync(request));
      response.Status = OperationResultStatusType.FullSuccess;

      if (!response.Body)
      {
        response = _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.BadRequest);
      }

      return response;
    }
  }
}
