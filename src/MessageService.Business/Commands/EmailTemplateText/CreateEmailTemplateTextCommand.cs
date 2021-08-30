using System;
using System.Collections.Generic;
using System.Net;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Business.Commands.EmailTemplateText.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Db.Interfaces;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.EmailTemplate;
using LT.DigitalOffice.MessageService.Validation.EmailTemplateText.Interfaces;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.MessageService.Business.Commands.EmailTemplateText
{
  public class CreateEmailTemplateTextCommand : ICreateEmailTemplateTextCommand
  {
    private readonly ICreateEmailTemplateTextValidator _validator;
    private readonly IDbEmailTemplateTextMapper _mapper;
    private readonly IEmailTemplateTextRepository _repository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateEmailTemplateTextCommand(
      ICreateEmailTemplateTextValidator validator,
      IDbEmailTemplateTextMapper mapper,
      IEmailTemplateTextRepository repository,
      IHttpContextAccessor httpContextAccessor)
    {
      _validator = validator;
      _mapper = mapper;
      _repository = repository;
      _httpContextAccessor = httpContextAccessor;
    }

    public OperationResultResponse<Guid?> Execute(EmailTemplateTextRequest request)
    {
      OperationResultResponse<Guid?> response = new();

      List<string> errors = new();

      if (!_validator.ValidateCustom(request, out errors))
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        response.Status = OperationResultStatusType.Failed;
        response.Errors.AddRange(errors);
        return response;
      }

      response.Body = _repository.Create(_mapper.Map(request));

      if (response.Body == null)
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        response.Status = OperationResultStatusType.Failed;
        return response;
      }

      _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;

      response.Status = OperationResultStatusType.FullSuccess;
      return response;
    }
  }
}
