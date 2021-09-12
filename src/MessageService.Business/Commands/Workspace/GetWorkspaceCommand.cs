using System;
using System.Collections.Generic;
using System.Linq;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Business.Commands.Workspace.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Models.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Models;
using LT.DigitalOffice.MessageService.Models.Dto.Models.Image;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace.Filters;
using LT.DigitalOffice.Models.Broker.Models;
using LT.DigitalOffice.Models.Broker.Requests.File;
using LT.DigitalOffice.Models.Broker.Requests.User;
using LT.DigitalOffice.Models.Broker.Responses.File;
using LT.DigitalOffice.Models.Broker.Responses.User;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace LT.DigitalOffice.MessageService.Business.Commands.Workspace
{
  public class GetWorkspaceCommand : IGetWorkspaceCommand
  {
    private readonly IWorkspaceInfoMapper _workspaceInfoMapper;
    private readonly IWorkspaceRepository _repository;
    private readonly IRequestClient<IGetImagesRequest> _rcGetImages;
    private readonly IRequestClient<IGetUsersDataRequest> _rcGetUsers;
    private readonly IImageInfoMapper _imageInfoMapper;
    private readonly ILogger<GetWorkspaceCommand> _logger;

    public GetWorkspaceCommand(
      IWorkspaceInfoMapper workspaceInfoMapper,
      IWorkspaceRepository repository,
      IRequestClient<IGetImagesRequest> rcGetImages,
      IRequestClient<IGetUsersDataRequest> rcGetUsers,
      IImageInfoMapper imageInfoMapper,
      ILogger<GetWorkspaceCommand> logger)
    {
      _workspaceInfoMapper = workspaceInfoMapper;
      _repository = repository;
      _rcGetImages = rcGetImages;
      _rcGetUsers = rcGetUsers;
      _imageInfoMapper = imageInfoMapper;
      _logger = logger;
    }

    private List<ImageInfo> GetImages(List<Guid> imageIds, List<string> errors)
    {
      if (imageIds == null || imageIds.Count == 0)
      {
        return null;
      }

      string errorMessage = "Cannot get images now. Please try again later.";
      const string logMessage = "Cannot get images with ids: {imageIds}.";

      try
      {
        IOperationResult<IGetImagesResponse> response = _rcGetImages.GetResponse<IOperationResult<IGetImagesResponse>>(
            IGetImagesRequest.CreateObj(imageIds)).Result.Message;

        if (response.IsSuccess)
        {
          return response.Body.Images.Select(_imageInfoMapper.Map).ToList();
        }

        _logger.LogWarning(logMessage, string.Join(", ", imageIds));
      }
      catch (Exception exc)
      {
        _logger.LogError(exc, logMessage, string.Join(", ", imageIds));
      }

      errors.Add(errorMessage);

      return null;
    }

    private List<UserData> GetUsers(List<Guid> userIds, List<string> errors)
    {
      if (userIds == null || userIds.Count == 0)
      {
        return null;
      }

      string errorMessage = "Cannot get users now. Please try again later.";
      const string logMessage = "Cannot get users with ids: {usersIds}.";

      try
      {
        IOperationResult<IGetUsersDataResponse> response = _rcGetUsers.GetResponse<IOperationResult<IGetUsersDataResponse>>(
            IGetUsersDataRequest.CreateObj(userIds)).Result.Message;

        if (response.IsSuccess)
        {
          return response.Body.UsersData;
        }

        _logger.LogWarning(logMessage, string.Join(", ", userIds));
      }
      catch (Exception exc)
      {
        _logger.LogError(exc, logMessage, string.Join(", ", userIds));
      }

      errors.Add(errorMessage);

      return null;
    }

    public OperationResultResponse<WorkspaceInfo> Execute(GetWorkspaceFilter filter)
    {
      DbWorkspace dbWorkspace = _repository.Get(filter);

      if (dbWorkspace == null)
      {
        return new OperationResultResponse<WorkspaceInfo>
        {
          Status = OperationResultStatusType.Failed,
          Errors = new() { $"Workspace with id: '{filter.WorkspaceId}' doesn't exist." }
        };
      }

      OperationResultResponse<WorkspaceInfo> response = new();

      List<UserData> usersData = GetUsers(dbWorkspace.Users?.Select(u => u.UserId).ToList(), response.Errors);

      List<Guid> imagesIds = new();

      if (usersData != null)
      {
        imagesIds.AddRange(usersData.Where(u => u.ImageId.HasValue).Select(u => u.ImageId.Value).ToList());
      }

      List<ImageInfo> images = GetImages(imagesIds, response.Errors);

      response.Status = response.Errors.Any() ? OperationResultStatusType.PartialSuccess : OperationResultStatusType.FullSuccess;
      response.Body = _workspaceInfoMapper.Map(dbWorkspace, images, usersData);

      return response;
    }
  }
}
