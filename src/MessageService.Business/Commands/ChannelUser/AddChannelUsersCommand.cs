using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentValidation.Results;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Business.Commands.ChannelUser.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Db.Interfaces;
using LT.DigitalOffice.MessageService.Validation.Validators.User.Interfaces;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.MessageService.Business.Commands.ChannelUser
{
  public class AddChannelUsersCommand : IAddChannelUsersCommand
  {
    private readonly IUserExistenceValidator _userExistenceValidator;
    private readonly IDbChannelUserMapper _dbChannelUserMapper;
    private readonly IChannelUserRepository _channelUserRepository;
    private readonly IResponseCreator _responseCreator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AddChannelUsersCommand(
      IUserExistenceValidator userExistenceValidator,
      IDbChannelUserMapper dbChannelUserMapper,
      IChannelUserRepository channelUserRepository,
      IResponseCreator responseCreator,
      IHttpContextAccessor httpContextAccessor)
    {
      _userExistenceValidator = userExistenceValidator;
      _dbChannelUserMapper = dbChannelUserMapper;
      _channelUserRepository = channelUserRepository;
      _responseCreator = responseCreator;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task<OperationResultResponse<bool>> ExecuteAsync(Guid channelId, List<Guid> usersIds)
    {
      Guid userId = _httpContextAccessor.HttpContext.GetUserId();

      if (!await _channelUserRepository.ChannelUserExistAsync(channelId, userId))
      {
        return _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.Forbidden);
      }

      ValidationResult validationResult = await _userExistenceValidator.ValidateAsync(usersIds);

      if (!validationResult.IsValid)
      {
        return _responseCreator.CreateFailureResponse<bool>(
          HttpStatusCode.BadRequest,
          validationResult.Errors.Select(e => e.ErrorMessage).ToList());
      }

      await _channelUserRepository.CreateAsync(
        usersIds.Select(id => _dbChannelUserMapper.Map(channelId, id, false, userId)).ToList());

      return new(body: true);
    }
  }
}
