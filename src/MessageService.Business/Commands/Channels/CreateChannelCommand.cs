using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentValidation.Results;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
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
    private readonly IResponseCreater _responseCreator;

    public CreateChannelCommand(
      IWorkspaceUserRepository workspaceUserRepository,
      ICreateChannelRequestValidator validator,
      IDbChannelMapper channelMapper,
      IChannelRepository channelRepository,
      IHttpContextAccessor httpContextAccessor,
      IResponseCreater responseCreator)
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
      Guid createdBy = _httpContextAccessor.HttpContext.GetUserId();

      DbWorkspaceUser dbWorkspaceCreator = await _workspaceUserRepository.GetAsync(request.WorkspaceId, createdBy);

      if (dbWorkspaceCreator is null)
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

      response.Body = await _channelRepository.CreateAsync(await _channelMapper.MapAsync(request, dbWorkspaceCreator));

      _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;
      response.Status = OperationResultStatusType.FullSuccess;

      if (response.Body is null)
      {
        _responseCreator.CreateFailureResponse<Guid?>(HttpStatusCode.BadRequest);
      }

      return response;
    }
  }
}
