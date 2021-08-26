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
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IRequestClient<IGetImagesRequest> _rcGetImages;
    private readonly IRequestClient<IGetUsersDataRequest> _rcGetUsers;
    private readonly IImageInfoMapper _imageInfoMapper;
    private readonly ILogger<GetWorkspaceCommand> _logger;

    public GetWorkspaceCommand(
      IWorkspaceInfoMapper workspaceInfoMapper,
      IWorkspaceRepository workspaceRepository,
      IRequestClient<IGetImagesRequest> rcGetImages,
      IRequestClient<IGetUsersDataRequest> rcGetUsers,
      IImageInfoMapper imageInfoMapper,
      ILogger<GetWorkspaceCommand> logger)
    {
      _workspaceInfoMapper = workspaceInfoMapper;
      _workspaceRepository = workspaceRepository;
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
      DbWorkspace workspace = _workspaceRepository.Get(filter);

      if (workspace == null)
      {
        return new OperationResultResponse<WorkspaceInfo>
        {
          Status = OperationResultStatusType.Failed,
          Body = null,
          Errors = new() { $"Workspace with id: '{filter.WorkspaceId}' doesn't exist." }
        };
      }

      List<Guid> usersIds = workspace.Users?.Select(u => u.Id).ToList() ?? new();
      usersIds.Add(workspace.CreatedBy);

      List<string> errors = new();

      List<UserData> users = GetUsers(usersIds, errors);

      List<Guid> imageIds = workspace.Channels?.Where(ch => ch.ImageId.HasValue).Select(ch => ch.ImageId.Value).ToList() ?? new();

      if (workspace.ImageId.HasValue)
      {
        imageIds.Add(workspace.ImageId.Value);
      }

      if (users != null)
      {
        imageIds.AddRange(users.Where(u => u.ImageId.HasValue).Select(u => u.ImageId.Value).ToList());
      }

      List<ImageInfo> images = GetImages(imageIds, errors);

      return new OperationResultResponse<WorkspaceInfo>
      {
        Status = errors.Any() ? OperationResultStatusType.PartialSuccess : OperationResultStatusType.FullSuccess,
        Body = _workspaceInfoMapper.Map(workspace, images, users),
        Errors = errors
      };
    }
  }
}
