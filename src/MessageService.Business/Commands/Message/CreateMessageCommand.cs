using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentValidation.Results;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Business.Commands.Message.Hubs;
using LT.DigitalOffice.MessageService.Business.Commands.Message.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Db.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Enums;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Message;
using LT.DigitalOffice.MessageService.Validation.Validators.Message.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace LT.DigitalOffice.MessageService.Business.Commands.Message
{
  public class CreateMessageCommand : ICreateMessageCommand
  {
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAccessValidator _accessValidator;
    private readonly ICreateMessageRequestValidator _validator;
    private readonly IMessageRepository _repository;
    private readonly IDbMessageMapper _mapper;
    private readonly IResponseCreater _responseCreator;
    private readonly IHubContext<ChatHub> _chatHub;
    private readonly ILogger<CreateMessageCommand> _logger;

    public CreateMessageCommand(
      IHttpContextAccessor httpContextAccessor,
      IAccessValidator accessValidator,
      ICreateMessageRequestValidator validator,
      IDbMessageMapper mapper,
      IMessageRepository repository,
      IResponseCreater responseCreator,
      IHubContext<ChatHub> chatHub,
      ILogger<CreateMessageCommand> logger)
    {
      _httpContextAccessor = httpContextAccessor;
      _accessValidator = accessValidator;
      _validator = validator;
      _mapper = mapper;
      _repository = repository;
      _responseCreator = responseCreator;
      _chatHub = chatHub;
      _logger = logger;
    }

    public async Task<OperationResultResponse<StatusType>> ExecuteAsync(CreateMessageRequest request)
    {
      ValidationResult validationResult = await _validator.ValidateAsync(request);

      if (!validationResult.IsValid)
      {
        return _responseCreator.CreateFailureResponse<StatusType>(
          HttpStatusCode.BadRequest,
          validationResult.Errors.Select(vf => vf.ErrorMessage).ToList());
      }

      OperationResultResponse<StatusType> response = new();

      DbMessage message = await _repository.CreateAsync(_mapper.Map(request));

      try
      {
        await _chatHub.Clients.Group(request.ChannelId.ToString()).SendAsync("RecieveMessage", new
        {
          Content = message.Content,
          Status = message.Status,
          UserId = message.CreatedBy,
          CreatedAtUtc = message.CreatedAtUtc
        });

        response.Body = StatusType.Sent;
        response.Status = OperationResultStatusType.FullSuccess;

        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;
      }
      catch (Exception ex)
      {
        _logger.LogError("Can't send message.", ex);
        response = _responseCreator.CreateFailureResponse<StatusType>(HttpStatusCode.InternalServerError);
      }

      return response;
    }
  }
}
