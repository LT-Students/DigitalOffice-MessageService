using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
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
using System;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.MessageService.Business.Commands.Workspace
{
    public class FindWorkspaceCommand : IFindWorkspaceCommand
    {
        private readonly IRequestClient<IGetImagesRequest> _rcGetImages;
        private readonly IWorkspaceRepository _workspaceRepository;
        private readonly IImageInfoMapper _imageInfoMapper;
        private readonly IWorkspaceInfoMapper _workspaceInfoMapper;
        private readonly IAccessValidator _accessValidator;
        private readonly ILogger<FindWorkspaceCommand> _logger;

        private List<ImageInfo> GetImages(List<Guid> imageIds, List<string> errors)
        {
            if (imageIds == null || imageIds.Count == 0)
            {
                return new();
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
            catch(Exception exc)
            {
                _logger.LogError(exc, logMessage, string.Join(", ", imageIds));
            }

            errors.Add(errorMessage);

            return new();
        }

        public FindWorkspaceCommand(
            IRequestClient<IGetImagesRequest> rcGetImages,
            IWorkspaceRepository workspaceRepository,
            IImageInfoMapper imageInfoMapper,
            IWorkspaceInfoMapper workspaceInfoMapper,
            IAccessValidator accessValidator,
            ILogger<FindWorkspaceCommand> logger)
        {
            _rcGetImages = rcGetImages;
            _workspaceInfoMapper = workspaceInfoMapper;
            _workspaceRepository = workspaceRepository;
            _imageInfoMapper = imageInfoMapper;
            _accessValidator = accessValidator;
            _logger = logger;
        }

        public FindResultResponse<WorkspaceInfo> Execute(FindWorkspaceFilter filter)
        {
            List<DbWorkspace> dbWorkspaces = _workspaceRepository.Find(filter, out int totalCount);

            List<Guid> imageIds = dbWorkspaces
                .Where(w => w.ImageId.HasValue)
                .Select(w => w.ImageId.Value)
                .ToList();

            imageIds.AddRange(
                dbWorkspaces
                    .SelectMany(w => w.Channels)
                    .Where(ch => ch.ImageId.HasValue)
                    .Select(ch => ch.ImageId.Value)
                    .ToList());

            List<string> errors = new();

            List<ImageInfo> imageInfos = GetImages(imageIds, errors);

            return new FindResultResponse<WorkspaceInfo>
            {
                Status = errors.Any() ? OperationResultStatusType.PartialSuccess : OperationResultStatusType.FullSuccess,
                TotalCount = totalCount,
                Body = dbWorkspaces.Select(w => _workspaceInfoMapper.Map(w, imageInfos)).ToList(),
                Errors = errors
            };
        }
    }
}
