using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentValidation.Results;
using LT.DigitalOffice.Kernel.BrokerSupport.Broker;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Business.Commands.Message.Hubs;
using LT.DigitalOffice.MessageService.Business.Commands.Message.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Db.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Models.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Configurations;
using LT.DigitalOffice.MessageService.Models.Dto.Enums;
using LT.DigitalOffice.MessageService.Models.Dto.Models.User;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Message;
using LT.DigitalOffice.MessageService.Models.Dto.Responses.Message;
using LT.DigitalOffice.MessageService.Validation.Validators.Message.Interfaces;
using LT.DigitalOffice.Models.Broker.Enums;
using LT.DigitalOffice.Models.Broker.Models;
using LT.DigitalOffice.Models.Broker.Requests.Image;
using LT.DigitalOffice.Models.Broker.Requests.User;
using LT.DigitalOffice.Models.Broker.Responses.Image;
using LT.DigitalOffice.Models.Broker.Responses.User;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LT.DigitalOffice.MessageService.Business.Commands.Message
{
  public class CreateMessageCommand : ICreateMessageCommand
  {
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ICreateMessageRequestValidator _validator;
    private readonly IMessageRepository _repository;
    private readonly IDbMessageMapper _mapper;
    private readonly IUserInfoMapper _userInfoMapper;
    private readonly IImageInfoMapper _imageInfoMapper;
    private readonly IResponseCreator _responseCreator;
    private readonly IHubContext<ChatHub> _chatHub;
    private readonly ILogger<CreateMessageCommand> _logger;
    private readonly IRequestClient<IGetUsersDataRequest> _rcGetUsers;
    private readonly IRequestClient<IGetImagesRequest> _rcGetImages;
    private readonly IMemoryCache _cache;
    private readonly IOptions<MemoryCacheConfig> _cacheOptions;

    private async Task<UserInfo> GetUserAsync(List<string> errors)
    {
      Guid userId = _httpContextAccessor.HttpContext.GetUserId();

      if (_cache.TryGetValue(userId, out UserInfo userInfo))
      {
        return userInfo;
      }

      UserData userData = (await GetUsersThroughBrokerAsync(new List<Guid>() { userId }, errors))
        ?.FirstOrDefault();

      ImageData imageData = userData is not null && userData.ImageId.HasValue
        ? (await GetImagesThroughBrokerAsync(new List<Guid>() { userData.ImageId.Value }, errors))?.FirstOrDefault()
        : null;
     
      if (userData is not null)
      {
        _cache.Set(
          userData.Id,
          _userInfoMapper.Map(userData, _imageInfoMapper.Map(imageData)),
          TimeSpan.FromMinutes(_cacheOptions.Value.CacheLiveInMinutes));
      }

      return userInfo;
    }

    private async Task<List<UserData>> GetUsersThroughBrokerAsync(List<Guid> usersIds, List<string> errors)
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

        if (response.Message.IsSuccess && response.Message.Body.UsersData.Any())
        {
          return response.Message.Body.UsersData;
        }

        _logger.LogWarning(
          "Error while getting users data with users ids: {UsersIds}.\nErrors: {Errors}",
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

    private async Task<List<ImageData>> GetImagesThroughBrokerAsync(List<Guid> imagesIds, List<string> errors)
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

        if (response.Message.IsSuccess && response.Message.Body.ImagesData.Any())
        {
          return response.Message.Body.ImagesData;
        }

        _logger.LogWarning(
          "Error while getting images with images ids: {ImagesIds}.\nErrors: {Errors}",
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

    public CreateMessageCommand(
      IHttpContextAccessor httpContextAccessor,
      ICreateMessageRequestValidator validator,
      IDbMessageMapper mapper,
      IUserInfoMapper userInfoMapper,
      IImageInfoMapper imageInfoMapper,
      IMessageRepository repository,
      IResponseCreator responseCreator,
      IHubContext<ChatHub> chatHub,
      ILogger<CreateMessageCommand> logger,
      IRequestClient<IGetUsersDataRequest> rcGetUsers,
      IRequestClient<IGetImagesRequest> rcGetImages,
      IMemoryCache cache,
      IOptions<MemoryCacheConfig> cacheOptions)
    {
      _httpContextAccessor = httpContextAccessor;
      _validator = validator;
      _mapper = mapper;
      _userInfoMapper = userInfoMapper;
      _imageInfoMapper = imageInfoMapper;
      _repository = repository;
      _responseCreator = responseCreator;
      _chatHub = chatHub;
      _logger = logger;
      _rcGetUsers = rcGetUsers;
      _rcGetImages = rcGetImages;
      _cache = cache;
      _cacheOptions = cacheOptions;
    }

    public async Task<OperationResultResponse<StatusType>> ExecuteAsync(CreateMessageRequest request)
    {
      ValidationResult validationResult = await _validator.ValidateAsync(request);

      if (!validationResult.IsValid)
      {
        return _responseCreator.CreateFailureResponse<StatusType>(
          HttpStatusCode.BadRequest,
          validationResult.Errors.Select(vf => vf.ErrorMessage).ToList());
      }

      OperationResultResponse<StatusType> response = new();

      DbMessage message = await _repository.CreateAsync(_mapper.Map(request));

      try
      {
        await _chatHub.Clients.Group(request.ChannelId.ToString()).SendAsync(
          "ReceiveMessage",
          new MessageInfo()
          {
            Id = message.Id,
            Content = message.Content,
            Status = message.Status,
            ThreadMessagesCount = message.ThreadMessagesCount,
            CreatedBy = await GetUserAsync(response.Errors),
            CreatedAtUtc = message.CreatedAtUtc,
            FilesIds = null, //to do
            Images = null //to do
          });

        response.Body = StatusType.Sent;
        response.Status = OperationResultStatusType.FullSuccess;

        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;
      }
      catch (Exception ex)
      {
        _logger.LogError("Can't send message.", ex);
        response = _responseCreator.CreateFailureResponse<StatusType>(HttpStatusCode.InternalServerError);
      }

      return response;
    }
  }
}
