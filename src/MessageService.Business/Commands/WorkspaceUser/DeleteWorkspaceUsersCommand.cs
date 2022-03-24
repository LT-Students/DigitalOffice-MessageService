using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Business.Commands.WorkspaceUser.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.MessageService.Business.Commands.WorkspaceUser
{
  public class DeleteWorkspaceUsersCommand : IDeleteWorkspaceUsersCommand
  {
    private readonly IWorkspaceUserRepository _repository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IResponseCreator _responseCreator;

    public DeleteWorkspaceUsersCommand(
      IWorkspaceUserRepository repository,
      IHttpContextAccessor httpContextAccessor,
      IResponseCreator responseCreator)
    {
      _repository = repository;
      _httpContextAccessor = httpContextAccessor;
      _responseCreator = responseCreator;
    }

    public async Task<OperationResultResponse<bool>> ExecuteAsync(Guid workspaceId, List<Guid> usersIds)
    {
      List<DbWorkspaceUser> users = await _repository.GetByWorkspaceIdAsync(workspaceId);

      Guid userId = _httpContextAccessor.HttpContext.GetUserId();

      if (!users.Any(u => u.IsAdmin && u.UserId == userId)
        && !usersIds.All(id => users.Any(u => u.CreatedBy == id)))
      {
        return _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.Forbidden);
      }

      await _repository.RemoveAsync(workspaceId, users);

      return new(true);
    }
  }
}
