using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentValidation.Results;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Business.Commands.WorkspaceUser.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Db.Interfaces;
using LT.DigitalOffice.MessageService.Validation.Validators.User.Interfaces;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.MessageService.Business.Commands.WorkspaceUser
{
  public class AddWorkspaceUsersCommand : IAddWorkspaceUsersCommand
  {
    private readonly IUserExistenceValidator _userExistenceValidator;
    private readonly IDbWorkspaceUserMapper _mapper;
    private readonly IWorkspaceUserRepository _repository;
    private readonly IResponseCreator _responseCreator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AddWorkspaceUsersCommand(
      IUserExistenceValidator userExistenceValidator,
      IDbWorkspaceUserMapper mapper,
      IWorkspaceUserRepository repository,
      IResponseCreator responseCreator,
      IHttpContextAccessor httpContextAccessor)
    {
      _userExistenceValidator = userExistenceValidator;
      _mapper = mapper;
      _repository = repository;
      _responseCreator = responseCreator;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task<OperationResultResponse<bool>> ExecuteAsync(Guid workspaceId, List<Guid> usersIds)
    {
      Guid userId = _httpContextAccessor.HttpContext.GetUserId();

      if (!await _repository.WorkspaceUsersExistAsync(new List<Guid> { userId }, workspaceId))
      {
        return _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.Forbidden);
      }

      ValidationResult validationResult = await _userExistenceValidator.ValidateAsync(usersIds);

      if (!validationResult.IsValid)
      {
        return _responseCreator.CreateFailureResponse<bool>(
          HttpStatusCode.BadRequest,
          validationResult.Errors.Select(e => e.ErrorMessage).ToList());
      }

      await _repository.CreateAsync(
        usersIds.Select(id => _mapper.Map(workspaceId, id, false, userId)).ToList());

      return new(true);
    }
  }
}
