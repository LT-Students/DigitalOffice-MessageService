using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.Broker.Responses;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.MessageService.Business.WorkspaceCommands.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.WorkspaceMappers.Interfaces;
using LT.DigitalOffice.MessageService.Models.Dto.Models;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace;
using LT.DigitalOffice.MessageService.Validation.Workspace.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;

namespace LT.DigitalOffice.MessageService.Business.WorkspaceCommands
{
    public class CreateWorkspaceCommand : ICreateWorkspaceCommand
    {
        private readonly IDbWorkspaceMapper _mapper;
        private readonly ICreateWorkspaceValidator _validator;
        private readonly IWorkspaceRepository _repository;
        private readonly ILogger<CreateWorkspaceCommand> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRequestClient<IAddImageRequest> _requestClient;

        private Guid? AddImageContent(ImageInfo image, Guid ownerId)
        {
            Guid? imageId = null;

            try
            {
                var imageRequest = IAddImageRequest.CreateObj(
                    image.Name,
                    image.Content,
                    image.Extension,
                    ownerId);
                var imageResponse = _requestClient.GetResponse<IOperationResult<IAddImageResponse>>(imageRequest).Result;
                if (imageResponse.Message.IsSuccess)
                {
                    imageId = imageResponse.Message.Body.Id;
                }
                else
                {
                    _logger.LogWarning(string.Join(", ", imageResponse.Message.Errors));
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Exception on create image request.");
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

        public Guid Execute(WorkspaceRequest workspace)
        {
            _validator.ValidateAndThrowCustom(workspace);

            var ownerId = _httpContextAccessor.HttpContext.GetUserId();

            Guid? imageId = null;
            if (workspace.Image != null)
            {
                imageId = AddImageContent(workspace.Image, ownerId);
            }

            return _repository.CreateWorkspace(_mapper.Map(workspace, ownerId, imageId));
        }
    }
}
