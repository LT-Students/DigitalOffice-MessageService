using FluentValidation;
using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.Broker.Responses;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.MessageService.Business.WorkspaceCommands.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.WorkspaceMappers.Interfaces;
using LT.DigitalOffice.MessageService.Models.Dto;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;

namespace LT.DigitalOffice.MessageService.Business.WorkspaceCommands
{
    public class CreateWorkspaceCommand : ICreateWorkspaceCommand
    {
        private readonly IWorkspaceRepository _repository;
        private readonly IValidator<Workspace> _validator;
        private readonly IDbWorkspaceMapper _mapper;
        private readonly IRequestClient<IAddImageRequest> _requestClient;
        private readonly ILogger<CreateWorkspaceCommand> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private Guid? AddImageContent(string imageContext)
        {
            Guid? imageId = null;

            try
            {
                var imageRequest = IAddImageRequest.CreateObj(imageContext);
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
            IWorkspaceRepository repository,
            IValidator<Workspace> validator,
            IDbWorkspaceMapper mapper,
            IRequestClient<IAddImageRequest> requestClient,
            ILogger<CreateWorkspaceCommand> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _validator = validator;
            _mapper = mapper;
            _requestClient = requestClient;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid Execute(Workspace workspace)
        {
            _validator.ValidateAndThrowCustom(workspace);

            var imageId = AddImageContent(workspace.Image);
            var ownerId = _httpContextAccessor.HttpContext.GetUserId();

            return _repository.CreateWorkspace(_mapper.Map(workspace, ownerId, imageId));
        }
    }
}
