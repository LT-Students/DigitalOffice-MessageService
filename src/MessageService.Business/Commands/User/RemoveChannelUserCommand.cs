using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Business.Commands.User.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;

namespace LT.DigitalOffice.MessageService.Business.Commands.User
{
  public class RemoveChannelUserCommand : IRemoveChannelUserCommand
  {
    private readonly IChannelUserRepository _repository;
    private readonly IAccessValidator _accessValidator;
    private readonly IResponseCreater _responseCreater;

    public RemoveChannelUserCommand(
      IChannelUserRepository repository,
      IAccessValidator accessValidator,
      IResponseCreater responseCreater)
    {
      _repository = repository;
      _accessValidator = accessValidator;
      _responseCreater = responseCreater;
    }

    public async Task<OperationResultResponse<bool>> ExecuteAsync(Guid channelId, List<Guid> usersIds)
    {
      if (!await _accessValidator.IsAdminAsync())
      {
        return _responseCreater.CreateFailureResponse<bool>(HttpStatusCode.Forbidden);
      }

      if (usersIds == null || !usersIds.Any())
      {
        return _responseCreater.CreateFailureResponse<bool>(HttpStatusCode.BadRequest);
      }

      OperationResultResponse<bool> response = new();

      response.Body = await _repository.RemoveAsync(channelId, usersIds);
      response.Status = OperationResultStatusType.FullSuccess;

      if (response.Body)
      {
        response = _responseCreater.CreateFailureResponse<bool>(HttpStatusCode.NotFound);
      }

      return response;
    }
  }
}
