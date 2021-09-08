using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Business.Commands.Workspace.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Db.Interfaces;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace;
using LT.DigitalOffice.MessageService.Validation.Validators.Workspace.Interfaces;
using LT.DigitalOffice.Models.Broker.Common;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace LT.DigitalOffice.MessageService.Business.Commands.Workspace
{
  public class CreateWorkspaceCommand : ICreateWorkspaceCommand
  {
    private readonly ICreateWorkspaceRequestValidator _validator;
    private readonly IDbWorkspaceMapper _mapper;
    private readonly IWorkspaceRepository _repository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRequestClient<ICheckUsersExistence> _rcCheckUsersExistence;
    private readonly ILogger<CreateWorkspaceCommand> _logger;

    private List<Guid> CheckUserExistence(List<Guid> usersIds, List<string> errors)
    {
      if (!usersIds.Any())
      {
        return usersIds;
      }

      string errorMessage = "Failed to check the existing users.";
      string logMessage = "Cannot check existing users withs this ids {userIds}";

      try
      {
        var response = _rcCheckUsersExistence.GetResponse<IOperationResult<ICheckUsersExistence>>(
          ICheckUsersExistence.CreateObj(usersIds)).Result;
        if (response.Message.IsSuccess)
        {
          return response.Message.Body.UserIds;
        }

        _logger.LogWarning("Can not find user Ids: {userIds}: " +
          $"{Environment.NewLine}{string.Join('\n', response.Message.Errors)}");
      }
      catch (Exception exc)
      {
        _logger.LogError(exc, logMessage);
      }

      errors.Add(errorMessage);
      return null;
    }

    public CreateWorkspaceCommand(
      ICreateWorkspaceRequestValidator validator,
      IDbWorkspaceMapper mapper,
      IWorkspaceRepository repository,
      IHttpContextAccessor httpContextAccessor,
      IRequestClient<ICheckUsersExistence> rcCheckUsersExistence,
      ILogger<CreateWorkspaceCommand> logger)
    {
      _validator = validator;
      _mapper = mapper;
      _repository = repository;
      _httpContextAccessor = httpContextAccessor;
      _rcCheckUsersExistence = rcCheckUsersExistence;
      _logger = logger;
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

      List<Guid> usersIds = CheckUserExistence(
        request.Users.Select(wu => wu.UserId).ToList(),
        response.Errors);

      response.Body = _repository.Add(_mapper.Map(request, usersIds));
      response.Status = OperationResultStatusType.FullSuccess;
      _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;

      if (response.Body == null)
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        response.Status = OperationResultStatusType.Failed;
      }

      return response;
    }
  }
}
