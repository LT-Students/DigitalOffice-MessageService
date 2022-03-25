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
    private readonly IWorkspaceUserRepository _workspaceUserRepository;
    private readonly IChannelUserRepository _channelUserRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IResponseCreator _responseCreator;

    public DeleteWorkspaceUsersCommand(
      IWorkspaceUserRepository workspaceUserRepository,
      IChannelUserRepository channelUserRepository,
      IHttpContextAccessor httpContextAccessor,
      IResponseCreator responseCreator)
    {
      _workspaceUserRepository = workspaceUserRepository;
      _channelUserRepository = channelUserRepository;
      _httpContextAccessor = httpContextAccessor;
      _responseCreator = responseCreator;
    }

    public async Task<OperationResultResponse<bool>> ExecuteAsync(Guid workspaceId, List<Guid> usersIds)
    {
      List<DbWorkspaceUser> users = await _workspaceUserRepository.GetAsync(workspaceId, usersIds);

      Guid userId = _httpContextAccessor.HttpContext.GetUserId();

      if (!users.Any(u => u.IsAdmin && u.UserId == userId)
        && !usersIds.All(id => users.Any(u => u.CreatedBy == id)))
      {
        return _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.Forbidden);
      }

      await _workspaceUserRepository.RemoveAsync(workspaceId, users);
      await _channelUserRepository.RemoveAsync(workspaceId, usersIds);

      return new(body: true);
    }
  }
}
