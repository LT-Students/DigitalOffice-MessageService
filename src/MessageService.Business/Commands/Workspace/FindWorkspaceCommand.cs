using System;
using System.Collections.Generic;
using System.Linq;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Business.Commands.Workspace.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Models.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Models;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace.Filters;
using LT.DigitalOffice.Models.Broker.Requests.File;
using LT.DigitalOffice.Models.Broker.Responses.File;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace LT.DigitalOffice.MessageService.Business.Commands.Workspace
{
  public class FindWorkspaceCommand : IFindWorkspaceCommand
  {
    private readonly IRequestClient<IGetImagesRequest> _rcGetImages;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IImageInfoMapper _imageInfoMapper;
    private readonly IShortWorkspaceInfoMapper _workspaceInfoMapper;
    private readonly ILogger<FindWorkspaceCommand> _logger;

    private List<ImageInfo> GetImages(List<Guid> imageIds, List<string> errors)
    {
      if (imageIds == null || imageIds.Count == 0)
      {
        return null;
      }

      string errorMessage = "Cannot get images now. Please try again later.";
      const string logMessage = "Cannot get images with ids: {imageIds}.";

      try
      {
        IOperationResult<IGetImagesResponse> response = _rcGetImages.GetResponse<IOperationResult<IGetImagesResponse>>(
            IGetImagesRequest.CreateObj(imageIds)).Result.Message;

        if (response.IsSuccess)
        {
          return response.Body.Images.Select(_imageInfoMapper.Map).ToList();
        }

        _logger.LogWarning(logMessage, string.Join(", ", imageIds));
      }
      catch (Exception exc)
      {
        _logger.LogError(exc, logMessage, string.Join(", ", imageIds));
      }

      errors.Add(errorMessage);

      return null;
    }

    public FindWorkspaceCommand(
        IRequestClient<IGetImagesRequest> rcGetImages,
        IWorkspaceRepository workspaceRepository,
        IImageInfoMapper imageInfoMapper,
        IShortWorkspaceInfoMapper workspaceInfoMapper,
        ILogger<FindWorkspaceCommand> logger)
    {
      _rcGetImages = rcGetImages;
      _workspaceInfoMapper = workspaceInfoMapper;
      _workspaceRepository = workspaceRepository;
      _imageInfoMapper = imageInfoMapper;
      _logger = logger;
    }

    public FindResultResponse<ShortWorkspaceInfo> Execute(FindWorkspaceFilter filter)
    {
      List<DbWorkspace> dbWorkspaces = _workspaceRepository.Find(filter, out int totalCount);

      List<Guid> imageIds = dbWorkspaces
          .Where(w => w.ImageId.HasValue)
          .Select(w => w.ImageId.Value)
          .ToList();

      List<string> errors = new();

      List<ImageInfo> imageInfos = GetImages(imageIds, errors);

      return new FindResultResponse<ShortWorkspaceInfo>
      {
        Status = errors.Any() ? OperationResultStatusType.PartialSuccess : OperationResultStatusType.FullSuccess,
        TotalCount = totalCount,
        Body = dbWorkspaces.Select(w => _workspaceInfoMapper.Map(w, imageInfos?.FirstOrDefault(i => i.Id == w.ImageId))).ToList(),
        Errors = errors
      };
    }
  }
}
