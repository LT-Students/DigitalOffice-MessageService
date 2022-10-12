using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentValidation.Results;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Business.Commands.Channel.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Db.Interfaces;
using LT.DigitalOffice.MessageService.Models.Dto.Requests;
using LT.DigitalOffice.MessageService.Validation.Validators.Channel.Interfaces;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.MessageService.Business.Commands.Channel
{
  public class CreateChannelCommand : ICreateChannelCommand
  {
    private readonly IWorkspaceUserRepository _workspaceUserRepository;
    private readonly ICreateChannelRequestValidator _validator;
    private readonly IDbChannelMapper _channelMapper;
    private readonly IChannelRepository _channelRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IResponseCreator _responseCreator;

    public CreateChannelCommand(
      IWorkspaceUserRepository workspaceUserRepository,
      ICreateChannelRequestValidator validator,
      IDbChannelMapper channelMapper,
      IChannelRepository channelRepository,
      IHttpContextAccessor httpContextAccessor,
      IResponseCreator responseCreator)
    {
      _workspaceUserRepository = workspaceUserRepository;
      _validator = validator;
      _channelMapper = channelMapper;
      _channelRepository = channelRepository;
      _httpContextAccessor = httpContextAccessor;
      _responseCreator = responseCreator;
    }

    public async Task<OperationResultResponse<Guid?>> ExeсuteAsync(CreateChannelRequest request)
    {
      if (!await _workspaceUserRepository.WorkspaceUsersExistAsync(
        new List<Guid>() { _httpContextAccessor.HttpContext.GetUserId() },
        request.WorkspaceId))
      {
        return _responseCreator.CreateFailureResponse<Guid?>(HttpStatusCode.Forbidden);
      }

      ValidationResult validationResult = await _validator.ValidateAsync(request);

      if (!validationResult.IsValid)
      {
        return _responseCreator.CreateFailureResponse<Guid?>(HttpStatusCode.BadRequest,
          validationResult.Errors.Select(vf => vf.ErrorMessage).ToList());
      }

      OperationResultResponse<Guid?> response = new();

      response.Body = await _channelRepository.CreateAsync(await _channelMapper.MapAsync(request));

      _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;

      if (response.Body is null)
      {
        response = _responseCreator.CreateFailureResponse<Guid?>(HttpStatusCode.BadRequest);
      }

      return response;
    }
  }
}
