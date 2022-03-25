using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Business.Commands.Message.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Message;
using LT.DigitalOffice.MessageService.Validation.Validators.Message.Interfaces;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.MessageService.Business.Commands.Message
{
  public class EditMessageCommand : IEditMessageCommand
  {
    private readonly IEditMessageRequestValidator _validator;
    private readonly IMessageRepository _repository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IResponseCreator _responseCreator;

    public EditMessageCommand(
      IEditMessageRequestValidator validator,
      IMessageRepository repository,
      IHttpContextAccessor httpContextAccessor,
      IResponseCreator responseCreator)
    {
      _validator = validator;
      _repository = repository;
      _httpContextAccessor = httpContextAccessor;
      _responseCreator = responseCreator;
    }

    public async Task<OperationResultResponse<bool>> ExecuteAsync(Guid messageId, EditMessageRequest request)
    {
      DbMessage message = await _repository.GetAsync(messageId);

      if (message is null)
      {
        return _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.NotFound);
      }

      if (message.CreatedBy != _httpContextAccessor.HttpContext.GetUserId())
      {
        return _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.Forbidden);
      }

      if (!_validator.ValidateCustom(request, out List<string> errors))
      {
        return _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.BadRequest, errors);
      }

      message.Content = request.Content;
      message.Status = (int)request.Status;
      message.ModifiedAtUtc = DateTime.UtcNow;
      message.ModifiedBy = _httpContextAccessor.HttpContext.GetUserId();

      bool result = await _repository.UpdateAsync(message);

      return new()
      {
        Body = result,
        Status = result ? OperationResultStatusType.FullSuccess : OperationResultStatusType.Failed,
        Errors = errors
      };
    }
  }
}
