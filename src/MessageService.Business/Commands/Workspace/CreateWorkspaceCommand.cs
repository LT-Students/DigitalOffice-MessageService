using System;
using System.Collections.Generic;
using System.Linq;
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
using LT.DigitalOffice.MessageService.Validation.Validators.Workspace.Interfaces;
using LT.DigitalOffice.Models.Broker.Requests.File;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace LT.DigitalOffice.MessageService.Business.Commands.Workspace
{
  public class CreateWorkspaceCommand : ICreateWorkspaceCommand
  {
    private readonly IDbWorkspaceMapper _mapper;
    private readonly ICreateWorkspaceRequestValidator _validator;
    private readonly IWorkspaceRepository _repository;
    private readonly ILogger<CreateWorkspaceCommand> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRequestClient<IAddImageRequest> _requestClient;

    private Guid? AddImageContent(CreateImageRequest image, Guid ownerId, List<string> errors)
    {
      Guid? imageId = null;

      string errorMessage = "Can not add image to the workspace. Please try again later.";

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
          return imageResponse.Message.Body;
        }

        errors.AddRange(imageResponse.Message.Errors);
        _logger.LogWarning(errorMessage);
      }
      catch (Exception exception)
      {
        _logger.LogError(exception, errorMessage);
      }

      errors.Add(errorMessage);

      return imageId;
    }

    public CreateWorkspaceCommand(
        IDbWorkspaceMapper mapper,
        ICreateWorkspaceRequestValidator validator,
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
