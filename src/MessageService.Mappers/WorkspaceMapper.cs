using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Exceptions;
using LT.DigitalOffice.MessageService.Mappers.Interfaces;
using LT.DigitalOffice.Broker.Responses;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;

namespace LT.DigitalOffice.MessageService.Mappers
{
    public class WorkspaceMapper : IMapper<Workspace, DbWorkspace>
    {
        private readonly IRequestClient<ICreateImageRequest> _client;
        private readonly ILogger _logger;

        public WorkspaceMapper(
            IRequestClient<ICreateImageRequest> client,
            ILogger<WorkspaceMapper> logger)
        {
            _client = client;
            _logger = logger;
        }

        public DbWorkspace Map(Workspace value)
        {
            if (value == null)
            {
                throw new BadRequestException();
            }

            Guid? imageId = null;

            try
            {
                var imageRequest = ICreateImageRequest.CreateObj(value.Image);
                var imageResponse = _client.GetResponse<IOperationResult<ICreateImageResponse>>(imageRequest).Result;
                imageId = imageResponse.Message.Body.Id;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Exception on create image request.");
            }

            return new DbWorkspace
            {
                Id = Guid.NewGuid(),
                Name = value.Name,
                Description = value.Description,
                ImageId = imageId,
                IsActive = true
            };
        }
    }
}
