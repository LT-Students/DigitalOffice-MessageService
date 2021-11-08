using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentValidation.Results;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Business.Commands.User.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Db.Interfaces;
using LT.DigitalOffice.MessageService.Validation.Validators.User.Interfaces;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.MessageService.Business.Commands.User
{
  public class CreateWorkspaceUsersCommand : ICreateWorkspaceUsersCommand
  {
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAccessValidator _accessValidator;
    private readonly ICreateWorkspaceUserValidator _validator;
    private readonly IDbWorkspaceUserMapper _mapper;
    private readonly IWorkspaceUserRepository _repository;
    private readonly IResponseCreater _responseCreater;

    public CreateWorkspaceUsersCommand(
      IHttpContextAccessor httpContextAccessor,
      IAccessValidator accessValidator,
      ICreateWorkspaceUserValidator validator,
      IDbWorkspaceUserMapper mapper,
      IWorkspaceUserRepository repository,
      IResponseCreater responseCreater)
    {
      _httpContextAccessor = httpContextAccessor;
      _accessValidator = accessValidator;
      _validator = validator;
      _mapper = mapper;
      _repository = repository;
      _responseCreater = responseCreater;
    }

    public async Task<OperationResultResponse<bool>> ExecuteAsync(Guid workspaceId, List<Guid> usersIds)
    {
      List<string> errors = new();

      if (!await _accessValidator.IsAdminAsync()
        && !await _repository.IsWorkspaceAdminAsync(workspaceId, _httpContextAccessor.HttpContext.GetUserId()))
      {
        return _responseCreater.CreateFailureResponse<bool>(HttpStatusCode.Forbidden);
      }

      ValidationResult validationResult = await _validator.ValidateAsync(usersIds);

      errors.AddRange(validationResult.Errors.Select(e => e.ErrorMessage).ToList());

      if (!validationResult.IsValid)
      {
        return _responseCreater.CreateFailureResponse<bool>(HttpStatusCode.BadRequest, errors);
      }

      OperationResultResponse<bool> response = new();

      response.Body = await _repository.CreateAsync(
        usersIds.Select(userId => _mapper.Map(userId, workspaceId, true, _httpContextAccessor.HttpContext.GetUserId())).ToList());

      _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;

      response.Status = OperationResultStatusType.FullSuccess;

      if (!response.Body)
      {
        response = _responseCreater.CreateFailureResponse<bool>(HttpStatusCode.BadRequest);
      }

      return response;
    }
  }
}
