using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Business.Commands.WorkspaceUser.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Patch.Interfaces;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.WorkspaceUser;
using LT.DigitalOffice.MessageService.Validation.Validators.WorkspaceUser.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.MessageService.Business.Commands.WorkspaceUser
{
  public class EditWorkspaceUserCommand : IEditWorkspaceUserCommand
  {
    private readonly IWorkspaceUserRepository _repository;
    private readonly IPatchDbWorkspaceUserMapper _mapper;
    private readonly IEditWorkspaceUserRequestValidator _validator;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IResponseCreator _responseCreator;

    public EditWorkspaceUserCommand(
      IWorkspaceUserRepository repository,
      IPatchDbWorkspaceUserMapper mapper,
      IEditWorkspaceUserRequestValidator validator,
      IHttpContextAccessor httpContextAccessor,
      IResponseCreator responseCreator)
    {
      _repository = repository;
      _mapper = mapper;
      _validator = validator;
      _httpContextAccessor = httpContextAccessor;
      _responseCreator = responseCreator;
    }

    public async Task<OperationResultResponse<bool>> ExecuteAsync(
      Guid workspaceId, Guid userId, JsonPatchDocument<EditWorkspaceUserRequest> document)
    {
      if (!await _repository.IsAdminAsync(workspaceId, _httpContextAccessor.HttpContext.GetUserId()))
      {
        return _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.Forbidden);
      }

      if (!_validator.ValidateCustom(document, out List<string> errors))
      {
        return _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.BadRequest, errors);
      }

      await _repository.EditAsync(
        workspaceId, userId, _mapper.Map(document));

      return new(body: true, errors: errors);
    }
  }
}
