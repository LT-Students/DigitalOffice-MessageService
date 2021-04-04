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
        private readonly IRequestClient<ICreateImageRequest> _requestClient;
        private readonly ILogger<CreateWorkspaceCommand> _logger;
        private readonly HttpContext _httpContext;

        private Guid? CreateImageRequest(string image)
        {
            Guid? imageId = null;

            try
            {
                var imageRequest = ICreateImageRequest.CreateObj(image);
                var imageResponse = _requestClient.GetResponse<IOperationResult<ICreateImageResponse>>(imageRequest).Result;
                imageId = imageResponse.Message.Body.Id;
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
            IRequestClient<ICreateImageRequest> requestClient,
            ILogger<CreateWorkspaceCommand> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _validator = validator;
            _mapper = mapper;
            _requestClient = requestClient;
            _logger = logger;
            _httpContext = httpContextAccessor.HttpContext;
        }

        public Guid Execute(Workspace workspace)
        {
            _validator.ValidateAndThrowCustom(workspace);

            var imageId = CreateImageRequest(workspace.Image);
            var ownerId = _httpContext.GetUserId();

            return _repository.CreateWorkspace(_mapper.Map(workspace, ownerId, imageId));
        }
    }
}
