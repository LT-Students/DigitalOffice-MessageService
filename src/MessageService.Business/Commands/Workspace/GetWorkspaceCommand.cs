using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Business.Commands.Workspace.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Models.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Models;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace.Filters;
using LT.DigitalOffice.Models.Broker.Models;
using LT.DigitalOffice.Models.Broker.Requests.User;
using LT.DigitalOffice.Models.Broker.Responses.User;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace LT.DigitalOffice.MessageService.Business.Commands.Workspace
{
  public class GetWorkspaceCommand : IGetWorkspaceCommand
  {
    private readonly IWorkspaceInfoMapper _workspaceInfoMapper;
    private readonly IWorkspaceRepository _repository;
    private readonly IRequestClient<IGetUsersDataRequest> _rcGetUsers;
    private readonly IImageInfoMapper _imageInfoMapper;
    private readonly ILogger<GetWorkspaceCommand> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IResponseCreater _responseCreator;

    public GetWorkspaceCommand(
      IWorkspaceInfoMapper workspaceInfoMapper,
      IWorkspaceRepository repository,
      IRequestClient<IGetUsersDataRequest> rcGetUsers,
      IImageInfoMapper imageInfoMapper,
      ILogger<GetWorkspaceCommand> logger,
      IHttpContextAccessor httpContextAccessor,
      IResponseCreater responseCreator)
    {
      _workspaceInfoMapper = workspaceInfoMapper;
      _repository = repository;
      _rcGetUsers = rcGetUsers;
      _imageInfoMapper = imageInfoMapper;
      _logger = logger;
      _responseCreator = responseCreator;
      _responseCreator = responseCreator;
    }

    private async Task<List<UserData>> GetUsers(List<Guid> usersIds, List<string> errors)
    {
      if (usersIds == null || !usersIds.Any())
      {
        return null;
      }

      string errorMessage = "Cannot get users now. Please try again later.";
      const string logMessage = "Cannot get users with ids: {usersIds}.";

      try
      {
        Response<IOperationResult<IGetUsersDataResponse>> response =
          await _rcGetUsers.GetResponse<IOperationResult<IGetUsersDataResponse>>(
            IGetUsersDataRequest.CreateObj(usersIds));

        if (response.Message.IsSuccess)
        {
          return response.Message.Body.UsersData;
        }

        _logger.LogWarning(logMessage, string.Join(", ", usersIds));
      }
      catch (Exception exc)
      {
        _logger.LogError(exc, logMessage, string.Join(", ", usersIds));
      }

      errors.Add(errorMessage);

      return null;
    }

    public async Task<OperationResultResponse<WorkspaceInfo>> ExecuteAsync(GetWorkspaceFilter filter)
    {
      DbWorkspace dbWorkspace = await _repository.GetAsync(filter);

      if (dbWorkspace is null)
      {
        return _responseCreator.CreateFailureResponse<WorkspaceInfo>(HttpStatusCode.NotFound);
      }

      OperationResultResponse<WorkspaceInfo> response = new();

      List<UserData> usersData = await GetUsers(dbWorkspace.Users?.Select(u => u.UserId).ToList(), response.Errors);

      List<Guid> imagesIds = new();

      if (usersData is not null)
      {
        imagesIds.AddRange(usersData.Where(u => u.ImageId.HasValue).Select(u => u.ImageId.Value).ToList());
      }

      response.Status = response.Errors.Any() ? OperationResultStatusType.PartialSuccess : OperationResultStatusType.FullSuccess;
      response.Body = _workspaceInfoMapper.Map(dbWorkspace, usersData);

      return response;
    }
  }
}
