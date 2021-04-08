﻿using LT.DigitalOffice.Kernel.Exceptions;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.MessageService.Business.WorkspaceCommands.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Models.Dto.Enums;
using LT.DigitalOffice.MessageService.Models.Dto.Responses;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace LT.DigitalOffice.MessageService.Business.WorkspaceCommands
{
    public class RemoveWorkspaceCommand : IRemoveWorkspaceCommand
    {
        private readonly IWorkspaceRepository _workspaceRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RemoveWorkspaceCommand(
            IWorkspaceRepository workspaceRepository,
            IUserRepository userRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _workspaceRepository = workspaceRepository;
            _userRepository = userRepository;
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
