using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Business.Commands.Channels.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Models.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Responses.Channel;
using LT.DigitalOffice.Models.Broker.Models;
using LT.DigitalOffice.Models.Broker.Requests.User;
using LT.DigitalOffice.Models.Broker.Responses.User;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace LT.DigitalOffice.MessageService.Business.Commands.Channels
{
  public class GetChannelCommand : IGetChannelCommand
  {
    private readonly IChannelInfoMapper _channelInfoMapper;
    private readonly IChannelRepository _channelRepository;
    private readonly IRequestClient<IGetUsersDataRequest> _rcGetUsers;
    private readonly ILogger<GetChannelCommand> _logger;
    private readonly IResponseCreater _responseCreator;

    private async Task<List<UserData>> GetUsersAsync(List<Guid> usersIds, List<string> errors)
    {
      if (usersIds is null || !usersIds.Any())
      {
        return null;
      }

      try
      {
        Response<IOperationResult<IGetUsersDataResponse>> response =
          await _rcGetUsers.GetResponse<IOperationResult<IGetUsersDataResponse>>(
            IGetUsersDataRequest.CreateObj(usersIds));

        if (response.Message.IsSuccess)
        {
          return response.Message.Body.UsersData;
        }

        _logger.LogWarning(
          "Error while geting users data with users ids: {UsersIds}.\nErrors: {Errors}",
          string.Join(", ", usersIds),
          string.Join('\n', response.Message.Errors));
      }
      catch (Exception exc)
      {
        _logger.LogError(
          exc,
          "Cannot get users data with users ids: {UsersIds}.",
          string.Join(", ", usersIds));
      }

      errors.Add("Cannot get users data. Please try again later.");

      return null;
    }

    public GetChannelCommand(
      IChannelInfoMapper channelInfoMapper,
      IChannelRepository channelRepository,
      IRequestClient<IGetUsersDataRequest> rcGetUsers,
      ILogger<GetChannelCommand> logger,
      IResponseCreater responseCreator)
    {
      _channelInfoMapper = channelInfoMapper;
      _channelRepository = channelRepository;
      _rcGetUsers = rcGetUsers;
      _logger = logger;
      _responseCreator = responseCreator;
    }

    public async Task<OperationResultResponse<ChannelInfo>> ExecuteAsync(Guid channelId)
    {
      DbChannel dbChannel = await _channelRepository.GetAsync(channelId);

      if (dbChannel is null)
      {
        return _responseCreator.CreateFailureResponse<ChannelInfo>(HttpStatusCode.NotFound);
      }

      OperationResultResponse<ChannelInfo> response = new();

      List<UserData> usersData = await GetUsersAsync(dbChannel.Users?.Select(u => u.WorkspaceUserId).ToList(), response.Errors);

      List<Guid> imagesIds = new();

      if (usersData is not null)
      {
        imagesIds.AddRange(usersData.Where(u => u.ImageId.HasValue).Select(u => u.ImageId.Value).ToList());
      }

      response.Status = response.Errors.Any() ? OperationResultStatusType.PartialSuccess : OperationResultStatusType.FullSuccess;
      response.Body = _channelInfoMapper.Map(dbChannel, usersData);

      return response;
    }
  }
}
