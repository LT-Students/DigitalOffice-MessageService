using System;
using System.Linq;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Business.Commands.Workspace.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.MessageService.Business.Commands.Workspace
{
  public class RemoveWorkspaceCommand : IRemoveWorkspaceCommand
  {
    private readonly IWorkspaceUserRepository _userRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RemoveWorkspaceCommand(
        IWorkspaceUserRepository userRepository,
        IHttpContextAccessor httpContextAccessor,
        IWorkspaceRepository workspaceRepository)
    {
      _userRepository = userRepository;
      _workspaceRepository = workspaceRepository;
      _httpContextAccessor = httpContextAccessor;
    }

    public OperationResultResponse<bool> Execute(Guid workspaceId)
    {
      Guid requesterId = _httpContextAccessor.HttpContext.GetUserId();

      DbWorkspace workspace = _workspaceRepository.Get(workspaceId);

      if (workspace == null)
      {
        return new OperationResultResponse<bool>
        {
          Status = OperationResultStatusType.Failed,
          Body = false,
          Errors = new() { $"Workspace with id: '{workspaceId}' doesn't exist." }
        };
      }

      if (requesterId != workspace.CreatedBy
          && _userRepository.GetAdmins(workspaceId).FirstOrDefault(wa => wa.UserId == requesterId) == null)
      {
        throw new ForbiddenException("Not enough rights.");
      }

      bool result = _workspaceRepository.SwitchActiveStatus(workspaceId, false);

      return new OperationResultResponse<bool>
      {
        Status = result ? OperationResultStatusType.FullSuccess : OperationResultStatusType.Failed,
        Body = result
      };
    }
  }
}
