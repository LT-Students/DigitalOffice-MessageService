using System;
using System.Collections.Generic;
using System.Net;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Business.Commands.Channels.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Db.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests;
using LT.DigitalOffice.MessageService.Validation.Validators.Channel.Interfaces;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.MessageService.Business.Commands.Channels
{
  public class CreateChannelCommand : ICreateChannelCommand
  {
    private readonly IWorkspaceUserRepository _workspaceUserRepository;
    private readonly ICreateChannelRequestValidator _validator;
    private readonly IDbChannelMapper _channelMapper;
    private readonly IChannelRepository _channelRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateChannelCommand(
      IWorkspaceUserRepository workspaceUserRepository,
      ICreateChannelRequestValidator validator,
      IDbChannelMapper channelMapper,
      IChannelRepository channelRepository,
      IHttpContextAccessor httpContextAccessor)
    {
      _workspaceUserRepository = workspaceUserRepository;
      _validator = validator;
      _channelMapper = channelMapper;
      _channelRepository = channelRepository;
      _httpContextAccessor = httpContextAccessor;
    }

    public OperationResultResponse<Guid?> Exeсute(CreateChannelRequest request)
    {
      Guid createdBy = _httpContextAccessor.HttpContext.GetUserId();

      DbWorkspaceUser dbWorkspaceCreator = _workspaceUserRepository.Get(request.WorkspaceId, createdBy);

      if (dbWorkspaceCreator == null)
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;

        return new OperationResultResponse<Guid?>()
        {
          Status = OperationResultStatusType.Failed,
          Errors = new List<string>() { "Not enough rights." }
        };
      }

      if (!_validator.ValidateCustom(request, out List<string> errors))
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        return new OperationResultResponse<Guid?>()
        {
          Status = OperationResultStatusType.Failed,
          Errors = errors
        };
      }

      OperationResultResponse<Guid?> response = new();

      response.Body = _channelRepository.Add(_channelMapper.Map(request, dbWorkspaceCreator));

      _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;
      response.Status = OperationResultStatusType.FullSuccess;

      if (response.Body == null)
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        response.Status = OperationResultStatusType.Failed;
      }

      return response;
    }
  }
}
