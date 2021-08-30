using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Business.Commands.Workspace.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Patch.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace;
using LT.DigitalOffice.MessageService.Validation.Validators.Workspace.Interfaces;
using LT.DigitalOffice.Models.Broker.Requests.File;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LT.DigitalOffice.MessageService.Business.Commands.Workspace
{
  public class EditWorkspaceCommand : IEditWorkspaceCommand
  {
    private readonly IEditWorkspaceRequestValidator _validator;
    private readonly IPatchDbWorkspaceMapper _mapper;
    private readonly IWorkspaceRepository _repository;
    private readonly IAccessValidator _accessValidator;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRequestClient<IAddImageRequest> _rcAddImage;
    private readonly ILogger<EditWorkspaceCommand> _logger;

    private Guid? AddImage(CreateImageRequest image, Guid ownerId, List<string> errors)
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
        var imageResponse = _rcAddImage.GetResponse<IOperationResult<Guid>>(imageRequest).Result;
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

    public EditWorkspaceCommand(
      IEditWorkspaceRequestValidator validator,
      IPatchDbWorkspaceMapper mapper,
      IWorkspaceRepository repository,
      IAccessValidator accessValidator,
      IHttpContextAccessor httpContextAccessor,
      IRequestClient<IAddImageRequest> rcAddImage,
      ILogger<EditWorkspaceCommand> logger)
    {
      _validator = validator;
      _mapper = mapper;
      _repository = repository;
      _accessValidator = accessValidator;
      _httpContextAccessor = httpContextAccessor;
      _rcAddImage = rcAddImage;
      _logger = logger;
    }

    public OperationResultResponse<bool> Execute(Guid workspaceId, JsonPatchDocument<EditWorkspaceRequest> request)
    {
      DbWorkspace workspace = _repository.Get(workspaceId);

      Guid editorId = _httpContextAccessor.HttpContext.GetUserId();

      if (workspace.CreatedBy != editorId
        && !_accessValidator.IsAdmin())
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;

        return new OperationResultResponse<bool>
        {
          Status = OperationResultStatusType.Failed,
          Errors = new() { "Not enough rights." }
        };
      }

      if(!_validator.ValidateCustom(request, out List<string> errors))
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        return new OperationResultResponse<bool>
        {
          Status = OperationResultStatusType.Failed,
          Errors = errors
        };
      }

      var imageOperation = request.Operations.FirstOrDefault(o => o.path.EndsWith(nameof(EditWorkspaceRequest.Image), StringComparison.OrdinalIgnoreCase));
      Guid? imageId = null;

      if (imageOperation != null)
      {
        imageId = AddImage(JsonConvert.DeserializeObject<CreateImageRequest>(imageOperation.value?.ToString()), editorId, errors);
      }

      return new OperationResultResponse<bool>
      {
        Status = errors.Any() ? OperationResultStatusType.PartialSuccess : OperationResultStatusType.FullSuccess,
        Body = _repository.Edit(workspace, _mapper.Map(request, imageId)),
        Errors = errors
      };
    }
  }
}
