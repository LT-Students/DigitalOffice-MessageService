using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Business.Commands.ChannelUser.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Patch.Interfaces;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.ChannelUser;
using LT.DigitalOffice.MessageService.Validation.Validators.ChannelUser.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.MessageService.Business.Commands.ChannelUser
{
  public class EditChannelUserCommand : IEditChannelUserCommand
  {
    private readonly IChannelUserRepository _channelUserRepository;
    private readonly IPatchDbChannelUserMapper _patchDbChannelUserMapper;
    private readonly IEditChannelUserRequestValidator _validator;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IResponseCreator _responseCreator;

    public EditChannelUserCommand(
      IChannelUserRepository channelUserRepository,
      IPatchDbChannelUserMapper patchDbChannelUserMapper,
      IEditChannelUserRequestValidator validator,
      IHttpContextAccessor httpContextAccessor,
      IResponseCreator responseCreator)
    {
      _channelUserRepository = channelUserRepository;
      _patchDbChannelUserMapper = patchDbChannelUserMapper;
      _validator = validator;
      _httpContextAccessor = httpContextAccessor;
      _responseCreator = responseCreator;
    }

    public async Task<OperationResultResponse<bool>> ExecuteAsync(
      Guid channelId, Guid userId, JsonPatchDocument<EditChannelUserRequest> document)
    {
      if (!await _channelUserRepository.IsAdminAsync(channelId, _httpContextAccessor.HttpContext.GetUserId()))
      {
        return _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.Forbidden);
      }

      if (!_validator.ValidateCustom(document, out List<string> errors))
      {
        return _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.BadRequest, errors);
      }

      await _channelUserRepository.EditAsync(
        channelId, userId, _patchDbChannelUserMapper.Map(document));

      return new()
      {
        Body = true,
        Status = OperationResultStatusType.FullSuccess,
        Errors = errors
      };
    }
  }
}
