using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Business.Commands.Channels.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Patch.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Channel;
using LT.DigitalOffice.MessageService.Validation.Validators.Channel.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.MessageService.Business.Commands.Channels
{
  public class EditChannelCommand : IEditChannelCommand
  {
    private readonly IChannelRepository _repository;
    private readonly IPatchDbChannelMapper _mapper;
    private readonly IEditChannelRequestValidator _validator;
    private readonly IAccessValidator _accessValidator;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IResponseCreator _responseCreator;

    public EditChannelCommand(
      IChannelRepository repository,
      IPatchDbChannelMapper mapper,
      IEditChannelRequestValidator validator,
      IAccessValidator accessValidator,
      IHttpContextAccessor httpContextAccessor,
      IResponseCreator responseCreator)
    {
      _repository = repository;
      _mapper = mapper;
      _validator = validator;
      _accessValidator = accessValidator;
      _httpContextAccessor = httpContextAccessor;
      _responseCreator = responseCreator;
    }

    public async Task<OperationResultResponse<bool>> ExecuteAsync(
      Guid channelId, JsonPatchDocument<EditChannelRequest> document)
    {
      DbChannel channel = await _repository.GetAsync(channelId);

      Guid userId = _httpContextAccessor.HttpContext.GetUserId();

      if (channel.CreatedBy != userId
        && !channel.Users.Any(u => u.UserId == userId)
        && !(await _accessValidator.IsAdminAsync()))
      {
        return _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.Forbidden);
      }

      if (!_validator.ValidateCustom(document, out List<string> errors))
      {
        return _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.BadRequest, errors);
      }

      if (!await _repository.EditAsync(channel, _mapper.Map(document)))
      {
        return _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.BadRequest, errors);
      }

      return new OperationResultResponse<bool>
      {
        Body = true,
        Errors = errors,
        Status = Kernel.Enums.OperationResultStatusType.FullSuccess
      };
    }
  }
}
