using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Business.Commands.ChannelUser.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.MessageService.Business.Commands.ChannelUser
{
  public class DeleteChannelUsersCommand : IDeleteChannelUsersCommand
  {
    private readonly IChannelUserRepository _channelUserRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IResponseCreator _responseCreator;

    public DeleteChannelUsersCommand(
      IChannelUserRepository channelUserRepository,
      IHttpContextAccessor httpContextAccessor,
      IResponseCreator responseCreator)
    {
      _channelUserRepository = channelUserRepository;
      _httpContextAccessor = httpContextAccessor;
      _responseCreator = responseCreator;
    }

    public async Task<OperationResultResponse<bool>> ExecuteAsync(Guid channelId, List<Guid> usersIds)
    {
      List<DbChannelUser> users = await _channelUserRepository.GetByChannelIdAsync(channelId);

      Guid userId = _httpContextAccessor.HttpContext.GetUserId();

      if (!users.Any(u => u.IsAdmin && u.UserId == userId)
        && !usersIds.All(id => users.Any(u => u.CreatedBy == id)))
      {
        return _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.Forbidden);
      }

      await _channelUserRepository.RemoveAsync(
        channelId, users.Where(u => usersIds.Contains(u.UserId)).ToList());

      return new(true);
    }
  }
}
