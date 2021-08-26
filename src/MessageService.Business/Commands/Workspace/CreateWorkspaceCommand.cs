using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Business.Commands.Workspace.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Db.Workspace.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace;
using LT.DigitalOffice.MessageService.Validation.Workspace.Interfaces;
using LT.DigitalOffice.Models.Broker.Requests.File;
using LT.DigitalOffice.Models.Broker.Responses.File;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.MessageService.Business.Commands.Workspace
{
    public class CreateWorkspaceCommand : ICreateWorkspaceCommand
    {
        private readonly IDbWorkspaceMapper _mapper;
        private readonly ICreateWorkspaceValidator _validator;
        private readonly IWorkspaceRepository _repository;
        private readonly ILogger<CreateWorkspaceCommand> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRequestClient<IAddImageRequest> _requestClient;

        private Guid? AddImageContent(CreateImageRequest image, Guid ownerId, List<string> errors)
        {
            Guid? imageId = null;

            string errorMessage = "Can not add image to user with id {userId}. Please try again later.";

            try
            {
                var imageRequest = IAddImageRequest.CreateObj(
                    image.Name,
                    image.Content,
                    image.Extension,
                    ownerId);
                var imageResponse = _requestClient.GetResponse<IOperationResult<Guid>>(imageRequest).Result;
                if (imageResponse.Message.IsSuccess)
                {
                    imageId = imageResponse.Message.Body;
                }
                else
                {
                    _logger.LogWarning(errorMessage, ownerId);

                    errors.Add($"Can not add image to user with id {ownerId}. Please try again later.");
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, errorMessage, ownerId);

                errors.Add($"Can not add image to user with id {ownerId}. Please try again later.");
            }

            return imageId;
        }

        public CreateWorkspaceCommand(
            IDbWorkspaceMapper mapper,
            ICreateWorkspaceValidator validator,
            IWorkspaceRepository repository,
            ILogger<CreateWorkspaceCommand> logger,
            IHttpContextAccessor httpContextAccessor,
            IRequestClient<IAddImageRequest> requestClient)
        {
            _mapper = mapper;
            _logger = logger;
            _validator = validator;
            _repository = repository;
            _requestClient = requestClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public OperationResultResponse<Guid> Execute(CreateWorkspaceRequest workspace)
        {
            List<string> errors = new();

            _validator.ValidateAndThrowCustom(workspace);

            var ownerId = _httpContextAccessor.HttpContext.GetUserId();

            Guid? imageId = null;
            if (workspace.Image != null)
            {
                imageId = AddImageContent(workspace.Image, ownerId, errors);
            }

            DbWorkspace dbWorkspace = _mapper.Map(workspace, ownerId, imageId);

            _repository.Add(dbWorkspace);

            return new OperationResultResponse<Guid>
            {
                Status = errors.Any() ? OperationResultStatusType.PartialSuccess : OperationResultStatusType.FullSuccess,
                Body = dbWorkspace.Id,
                Errors = errors
            };
        }
    }
}
