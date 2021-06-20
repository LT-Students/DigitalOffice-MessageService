using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Business.Commands.Workspace.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace LT.DigitalOffice.MessageService.Business.Commands.Workspace
{
    public class RemoveWorkspaceCommand : IRemoveWorkspaceCommand
    {
        private readonly IUserRepository _userRepository;
        private readonly IWorkspaceRepository _workspaceRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RemoveWorkspaceCommand(
            IUserRepository userRepository,
            IHttpContextAccessor httpContextAccessor,
            IWorkspaceRepository workspaceRepository)
        {
            _userRepository = userRepository;
            _workspaceRepository = workspaceRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public OperationResultResponse<bool> Execute(Guid workspaceId)
        {
            var requesterId = _httpContextAccessor.HttpContext.GetUserId();

            if (requesterId != _workspaceRepository.GetWorkspace(workspaceId).OwnerId
                && _userRepository.GetAdmins(workspaceId).FirstOrDefault(wa => wa.UserId == requesterId) == null)
            {
                throw new ForbiddenException("Not enough rights.");
            }

            return new OperationResultResponse<bool>
            {
                Status = OperationResultStatusType.FullSuccess,
                Body = _workspaceRepository.SwitchActiveStatus(workspaceId, false)
            };
        }
    }
}
