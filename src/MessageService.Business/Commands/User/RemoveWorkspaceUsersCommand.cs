using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Business.Commands.User.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.MessageService.Business.Commands.User
{
  public class RemoveWorkspaceUsersCommand : IRemoveWorkspaceUsersCommand
  {
    private readonly IWorkspaceUserRepository _repository;
    private readonly IAccessValidator _accessValidator;
    private readonly IResponseCreater _responseCreater;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RemoveWorkspaceUsersCommand(
      IWorkspaceUserRepository repository,
      IAccessValidator accessValidator,
      IResponseCreater responseCreater,
      IHttpContextAccessor httpContextAccessor)
    {
      _repository = repository;
      _accessValidator = accessValidator;
      _responseCreater = responseCreater;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task<OperationResultResponse<bool>> ExecuteAsync(Guid workspaceId, List<Guid> usersIds)
    {
      if (!await _accessValidator.IsAdminAsync()
        && !await _repository.IsWorkspaceAdminAsync(workspaceId, _httpContextAccessor.HttpContext.GetUserId()))
      {
        return _responseCreater.CreateFailureResponse<bool>(HttpStatusCode.Forbidden);
      }

      if (usersIds == null || !usersIds.Any())
      {
        return _responseCreater.CreateFailureResponse<bool>(HttpStatusCode.BadRequest);
      }

      bool response = await _repository.RemoveAsync(workspaceId, usersIds);

      return new()
      {
        Status = response ? OperationResultStatusType.FullSuccess : OperationResultStatusType.Failed,
        Body = response
      };
    }
  }
}
