using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.BrokerSupport.Broker;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Business.Commands.Workspace.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Models.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Models.Image;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace.Filters;
using LT.DigitalOffice.MessageService.Models.Dto.Responses.Workspace;
using LT.DigitalOffice.Models.Broker.Enums;
using LT.DigitalOffice.Models.Broker.Models;
using LT.DigitalOffice.Models.Broker.Requests.Image;
using LT.DigitalOffice.Models.Broker.Requests.User;
using LT.DigitalOffice.Models.Broker.Responses.Image;
using LT.DigitalOffice.Models.Broker.Responses.User;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace LT.DigitalOffice.MessageService.Business.Commands.Workspace
{
  public class GetWorkspaceCommand : IGetWorkspaceCommand
  {
    private readonly IWorkspaceInfoMapper _workspaceInfoMapper;
    private readonly IImageInfoMapper _imageMapper;
    private readonly IUserInfoMapper _userMapper;
    private readonly IWorkspaceRepository _repository;
    private readonly IRequestClient<IGetUsersDataRequest> _rcGetUsers;
    private readonly IRequestClient<IGetImagesRequest> _rcGetImages;
    private readonly ILogger<GetWorkspaceCommand> _logger;
    private readonly IResponseCreator _responseCreator;
    private readonly IHttpContextAccessor _httpContextAccessor;

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

        if (response.Message.IsSuccess)
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

    public GetWorkspaceCommand(
      IWorkspaceInfoMapper workspaceInfoMapper,
      IImageInfoMapper imageMapper,
      IUserInfoMapper userMapper,
      IWorkspaceRepository repository,
      IRequestClient<IGetUsersDataRequest> rcGetUsers,
      IRequestClient<IGetImagesRequest> rcGetImages,
      ILogger<GetWorkspaceCommand> logger,
      IResponseCreator responseCreator,
      IHttpContextAccessor httpContextAccessor)
    {
      _workspaceInfoMapper = workspaceInfoMapper;
      _imageMapper = imageMapper;
      _userMapper = userMapper;
      _repository = repository;
      _rcGetUsers = rcGetUsers;
      _rcGetImages = rcGetImages;
      _logger = logger;
      _responseCreator = responseCreator;
      _responseCreator = responseCreator;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task<OperationResultResponse<WorkspaceInfo>> ExecuteAsync(GetWorkspaceFilter filter)
    {
      DbWorkspace dbWorkspace = await _repository.GetAsync(filter);

      if (dbWorkspace is null)
      {
        return _responseCreator.CreateFailureResponse<WorkspaceInfo>(HttpStatusCode.NotFound);
      }

      if (!dbWorkspace.Users.Select(u => u.UserId).Contains(_httpContextAccessor.HttpContext.GetUserId()))
      {
        return _responseCreator.CreateFailureResponse<WorkspaceInfo>(HttpStatusCode.Forbidden);
      }

      OperationResultResponse<WorkspaceInfo> response = new();

      List<UserData> usersData = await GetUsersAsync(dbWorkspace.Users?.Select(u => u.UserId).ToList(), response.Errors);

      List<ImageInfo> imagesInfo = (await GetImagesAsync(
          usersData?.Where(u => u.ImageId.HasValue).Select(u => u.ImageId.Value).ToList(),
          response.Errors))
        ?.Select(_imageMapper.Map).ToList();

      response.Status = response.Errors.Any() ? OperationResultStatusType.PartialSuccess : OperationResultStatusType.FullSuccess;
      response.Body = _workspaceInfoMapper.Map(
        dbWorkspace,
        usersData?.Select(u => _userMapper.Map(u, imagesInfo?.FirstOrDefault(i => i.Id == u.ImageId))).ToList());

      return response;
    }
  }
}
