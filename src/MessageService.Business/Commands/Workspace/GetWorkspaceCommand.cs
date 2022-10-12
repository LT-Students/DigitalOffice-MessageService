using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.BrokerSupport.Helpers;
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
    private readonly IWorkspaceUserRepository _userRepository;
    private readonly IRequestClient<IGetUsersDataRequest> _rcGetUsers;
    private readonly IRequestClient<IGetImagesRequest> _rcGetImages;
    private readonly ILogger<GetWorkspaceCommand> _logger;
    private readonly IResponseCreator _responseCreator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetWorkspaceCommand(
      IWorkspaceInfoMapper workspaceInfoMapper,
      IImageInfoMapper imageMapper,
      IUserInfoMapper userMapper,
      IWorkspaceRepository repository,
      IWorkspaceUserRepository userRepository,
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
      _userRepository = userRepository;
      _rcGetUsers = rcGetUsers;
      _rcGetImages = rcGetImages;
      _logger = logger;
      _responseCreator = responseCreator;
      _responseCreator = responseCreator;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task<OperationResultResponse<WorkspaceInfo>> ExecuteAsync(
      Guid workspaceId, GetWorkspaceFilter filter)
    {
      DbWorkspace dbWorkspace = await _repository.GetAsync(workspaceId, filter);

      if (dbWorkspace is null)
      {
        return _responseCreator.CreateFailureResponse<WorkspaceInfo>(HttpStatusCode.NotFound);
      }

      Guid userId = _httpContextAccessor.HttpContext.GetUserId();

      // maybe better always join users, it should be tested.
      if (!(dbWorkspace.Users.Any(u => u.UserId == userId))
        && !(!dbWorkspace.Users.Any()
          && await _userRepository.WorkspaceUsersExistAsync(new List<Guid>() { userId }, workspaceId)))
      {
        return _responseCreator.CreateFailureResponse<WorkspaceInfo>(HttpStatusCode.Forbidden);
      }

      OperationResultResponse<WorkspaceInfo> response = new();

      List<UserData> usersData =
        (await RequestHandler.ProcessRequest<IGetUsersDataRequest, IGetUsersDataResponse>(
          _rcGetUsers,
          IGetUsersDataRequest.CreateObj(dbWorkspace.Users?.Select(u => u.UserId).ToList()),
          response.Errors,
          _logger))
        ?.UsersData;

      List<Guid> imagesIds = usersData
        ?.Where(u => u.ImageId.HasValue)
        .Select(u => u.ImageId.Value)
        .ToList();

      List<ImageInfo> imagesInfo = null;

      if (imagesIds.Any())
      {
        imagesInfo =
          (await RequestHandler.ProcessRequest<IGetImagesRequest, IGetImagesResponse>(
            _rcGetImages,
            IGetImagesRequest.CreateObj(
              imagesIds,
              ImageSource.User),
            response.Errors,
            _logger))
          ?.ImagesData?.Select(_imageMapper.Map).ToList();
      }

      response.Body = _workspaceInfoMapper.Map(
        dbWorkspace,
        usersData?.Select(u => _userMapper.Map(u, imagesInfo?.FirstOrDefault(i => i.Id == u.ImageId))).ToList());

      return response;
    }
  }
}
