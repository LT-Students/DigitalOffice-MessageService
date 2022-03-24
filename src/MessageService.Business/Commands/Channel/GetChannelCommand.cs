using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.BrokerSupport.Helpers;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Kernel.Validators.Interfaces;
using LT.DigitalOffice.MessageService.Business.Commands.Channel.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Models.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Filtres;
using LT.DigitalOffice.MessageService.Models.Dto.Responses.Channel;
using LT.DigitalOffice.Models.Broker.Enums;
using LT.DigitalOffice.Models.Broker.Requests.Image;
using LT.DigitalOffice.Models.Broker.Requests.User;
using LT.DigitalOffice.Models.Broker.Responses.Image;
using LT.DigitalOffice.Models.Broker.Responses.User;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace LT.DigitalOffice.MessageService.Business.Commands.Channel
{
  public class GetChannelCommand : IGetChannelCommand
  {
    private readonly IBaseFindFilterValidator _baseFindValidator;
    private readonly IChannelRepository _repository;
    private readonly IChannelInfoMapper _channelMapper;
    private readonly IMessageInfoMapper _messageMapper;
    private readonly IUserInfoMapper _userMapper;
    private readonly IImageInfoMapper _imageMapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IResponseCreator _responseCreator;
    private readonly IRequestClient<IGetUsersDataRequest> _rcGetUsers;
    private readonly IRequestClient<IGetImagesRequest> _rcGetImages;
    private readonly ILogger<GetChannelCommand> _logger;

    public GetChannelCommand(
      IBaseFindFilterValidator baseFindValidator,
      IChannelRepository repository,
      IChannelInfoMapper channelMapper,
      IMessageInfoMapper messageMapper,
      IUserInfoMapper userMapper,
      IImageInfoMapper imageMapper,
      IHttpContextAccessor httpContextAccessor,
      IResponseCreator responseCreator,
      IRequestClient<IGetUsersDataRequest> rcGetUsers,
      IRequestClient<IGetImagesRequest> rcGetImages,
      ILogger<GetChannelCommand> logger)
    {
      _baseFindValidator = baseFindValidator;
      _repository = repository;
      _channelMapper = channelMapper;
      _messageMapper = messageMapper;
      _userMapper = userMapper;
      _imageMapper = imageMapper;
      _httpContextAccessor = httpContextAccessor;
      _responseCreator = responseCreator;
      _rcGetUsers = rcGetUsers;
      _rcGetImages = rcGetImages;
      _logger = logger;
    }

    public async Task<OperationResultResponse<ChannelInfo>> ExeсuteAsync(Guid channelId, GetChannelFilter filter)
    {
      if (!_baseFindValidator.ValidateCustom(filter, out List<string> errors))
      {
        return _responseCreator.CreateFailureResponse<ChannelInfo>(HttpStatusCode.BadRequest, errors);
      }

      DbChannel dbChannel = await _repository.GetAsync(channelId, filter);

      Guid requestUserId = _httpContextAccessor.HttpContext.GetUserId();

      if (dbChannel is null)
      {
        return _responseCreator.CreateFailureResponse<ChannelInfo>(HttpStatusCode.NotFound);
      }

      if (dbChannel.IsPrivate
        && !dbChannel.Users.Select(cu => cu.UserId).Contains(requestUserId)
        || !dbChannel.Workspace.Users.Select(wu => wu.UserId).Contains(requestUserId))
      {
        return _responseCreator.CreateFailureResponse<ChannelInfo>(HttpStatusCode.Forbidden);
      }

      OperationResultResponse<ChannelInfo> response = new();

      var usersIds = dbChannel.Users.Select(cu => cu.UserId).ToList();
      usersIds.AddRange(dbChannel.Messages.Select(m => m.CreatedBy).Distinct().ToList());

      var usersData =
        (await _rcGetUsers.ProcessRequest<IGetUsersDataRequest, IGetUsersDataResponse>(
          IGetUsersDataRequest.CreateObj(usersIds),
          response.Errors,
          _logger))?.UsersData;

      var imagesIds = usersData?.Where(u => u.ImageId.HasValue)?.Select(u => u.ImageId.Value).ToList();

      //add messages images to image service

      var imagesInfo =
        (await _rcGetImages.ProcessRequest<IGetImagesRequest, IGetImagesResponse>(
          IGetImagesRequest.CreateObj(imagesIds, ImageSource.User),
          response.Errors,
          _logger))
        ?.ImagesData
        .Select(_imageMapper.Map)
        .ToList();

      var usersInfo = usersData
        ?.Select(u =>
          _userMapper.Map(u, imagesInfo?.FirstOrDefault(i => i.Id == u.ImageId))).ToList();

      response.Body = _channelMapper.Map(
        dbChannel,
        dbChannel.Messages.Select(
          m => _messageMapper.Map(
            m,
            usersInfo?.FirstOrDefault(u => u.Id == m.CreatedBy),
            imagesInfo?.Where(i => m.Images.Select(mi => mi.ImageId).Contains(i.Id)).ToList())).ToList(),
        usersInfo?.Where(u => dbChannel.Users.Select(u => u.UserId).Contains(u.Id)).ToList());

      response.Status = OperationResultStatusType.FullSuccess;

      return response;
    }
  }
}
