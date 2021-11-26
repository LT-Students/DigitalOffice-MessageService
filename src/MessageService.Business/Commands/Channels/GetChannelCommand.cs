using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Kernel.Validators.Interfaces;
using LT.DigitalOffice.MessageService.Business.Commands.Channels.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Models.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Filtres;
using LT.DigitalOffice.MessageService.Models.Dto.Models.Image;
using LT.DigitalOffice.MessageService.Models.Dto.Models.User;
using LT.DigitalOffice.MessageService.Models.Dto.Responses.Channel;
using LT.DigitalOffice.Models.Broker.Enums;
using LT.DigitalOffice.Models.Broker.Models;
using LT.DigitalOffice.Models.Broker.Requests.Image;
using LT.DigitalOffice.Models.Broker.Requests.User;
using LT.DigitalOffice.Models.Broker.Responses.Image;
using LT.DigitalOffice.Models.Broker.Responses.User;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace LT.DigitalOffice.MessageService.Business.Commands.Channels
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
    private readonly IResponseCreater _responseCreator;
    private readonly IRequestClient<IGetUsersDataRequest> _rcGetUsers;
    private readonly IRequestClient<IGetImagesRequest> _rcGetImages;
    private readonly ILogger<GetChannelCommand> _logger;

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

        if (response.Message.IsSuccess && !response.Message.Body.UsersData.Any())
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

    private async Task<List<ImageData>> GetImagesAsync(List<Guid> imagesIds, List<string> errors)
    {
      if (imagesIds is null || !imagesIds.Any())
      {
        return null;
      }

      try
      {
        Response<IOperationResult<IGetImagesResponse>> response =
          await _rcGetImages.GetResponse<IOperationResult<IGetImagesResponse>>(
            IGetImagesRequest.CreateObj(imagesIds, ImageSource.User));

        if (response.Message.IsSuccess && !response.Message.Body.ImagesData.Any())
        {
          return response.Message.Body.ImagesData;
        }

        _logger.LogWarning(
          "Error while geting images with images ids: {ImagesIds}.\nErrors: {Errors}",
          string.Join(", ", imagesIds),
          string.Join('\n', response.Message.Errors));
      }
      catch (Exception exc)
      {
        _logger.LogError(
          exc,
          "Cannot get images with images ids: {ImagesIds}.",
          string.Join(", ", imagesIds));
      }

      errors.Add("Cannot get images. Please try again later.");

      return null;
    }

    public GetChannelCommand(
      IBaseFindFilterValidator baseFindValidator,
      IChannelRepository repository,
      IChannelInfoMapper channelMapper,
      IMessageInfoMapper messageMapper,
      IUserInfoMapper userMapper,
      IImageInfoMapper imageMapper,
      IHttpContextAccessor httpContextAccessor,
      IResponseCreater responseCreator,
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

    public async Task<OperationResultResponse<ChannelInfo>> ExeсuteAsync(GetChannelFilter filter)
    {
      if (!_baseFindValidator.ValidateCustom(filter, out List<string> errors))
      {
        return _responseCreator.CreateFailureResponse<ChannelInfo>(HttpStatusCode.BadRequest, errors);
      }

      DbChannel dbChannel = await _repository.GetAsync(filter);

      Guid requestUserId = _httpContextAccessor.HttpContext.GetUserId();

      if (dbChannel is null)
      {
        return _responseCreator.CreateFailureResponse<ChannelInfo>(HttpStatusCode.NotFound);
      }

      if ((dbChannel.IsPrivate
          && !dbChannel.Users.Select(cu => cu.UserId).Contains(requestUserId))
        || !dbChannel.Workspace.Users.Select(wu => wu.UserId).Contains(requestUserId))
      {
        return _responseCreator.CreateFailureResponse<ChannelInfo>(HttpStatusCode.Forbidden);
      }

      OperationResultResponse<ChannelInfo> response = new();

      List<Guid> usersIds = dbChannel.Users.Select(cu => cu.UserId).ToList();
      usersIds.AddRange(dbChannel.Messages.Select(m => m.CreatedBy).Distinct().ToList());

      List<UserData> usersData = await GetUsersAsync(usersIds, response.Errors);

      List<Guid> imagesIds = usersData?.Where(u => u.ImageId.HasValue)?.Select(u => u.ImageId.Value).ToList();

      //add messages immages to image service

      List<ImageInfo> imagesInfo = (await GetImagesAsync(imagesIds, response.Errors))
        ?.Select(_imageMapper.Map).ToList();

      List<UserInfo> usersInfo = usersData
        ?.Select(u =>
          _userMapper.Map(u, imagesInfo?.FirstOrDefault(i => i.Id == u.ImageId))).ToList();

      response.Body = _channelMapper.Map(
        dbChannel,
        dbChannel.Messages.Select(
          m => _messageMapper.Map(
            m,
            usersInfo?.FirstOrDefault(u => u.Id == m.CreatedBy),
            imagesInfo?.Where(i => m.Images.Select(mi => mi.ImageId).Contains(i.Id)).ToList())).ToList(),
        usersInfo?.Where(u => dbChannel.Users.Select(u => u.WorkspaceUser.UserId).Contains(u.Id)).ToList());

      response.Status = OperationResultStatusType.FullSuccess;

      return response;
    }
  }
}
