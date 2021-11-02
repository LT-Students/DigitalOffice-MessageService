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
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace.Filters;
using LT.DigitalOffice.MessageService.Models.Dto.Responses.Workspace;
using LT.DigitalOffice.Models.Broker.Models;
using LT.DigitalOffice.Models.Broker.Requests.User;
using LT.DigitalOffice.Models.Broker.Responses.User;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace LT.DigitalOffice.MessageService.Business.Commands.Workspace
{
  public class GetWorkspaceCommand : IGetWorkspaceCommand
  {
    private readonly IWorkspaceInfoMapper _workspaceInfoMapper;
    private readonly IWorkspaceRepository _repository;
    private readonly IRequestClient<IGetUsersDataRequest> _rcGetUsers;
    private readonly ILogger<GetWorkspaceCommand> _logger;
    private readonly IResponseCreater _responseCreator;

    public GetWorkspaceCommand(
      IWorkspaceInfoMapper workspaceInfoMapper,
      IWorkspaceRepository repository,
      IRequestClient<IGetUsersDataRequest> rcGetUsers,
      ILogger<GetWorkspaceCommand> logger,
      IResponseCreater responseCreator)
    {
      _workspaceInfoMapper = workspaceInfoMapper;
      _repository = repository;
      _rcGetUsers = rcGetUsers;
      _logger = logger;
      _responseCreator = responseCreator;
      _responseCreator = responseCreator;
    }

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

    public async Task<OperationResultResponse<WorkspaceInfo>> ExecuteAsync(GetWorkspaceFilter filter)
    {
      DbWorkspace dbWorkspace = await _repository.GetAsync(filter);

      if (dbWorkspace is null)
      {
        return _responseCreator.CreateFailureResponse<WorkspaceInfo>(HttpStatusCode.NotFound);
      }

      OperationResultResponse<WorkspaceInfo> response = new();

      List<UserData> usersData = await GetUsersAsync(dbWorkspace.Users?.Select(u => u.UserId).ToList(), response.Errors);

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
