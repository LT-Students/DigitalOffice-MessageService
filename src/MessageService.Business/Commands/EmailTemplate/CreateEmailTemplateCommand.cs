using System;
using System.Collections.Generic;
using System.Net;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Business.Commands.EmailTemplate.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Db.Interfaces;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.EmailTemplate;
using LT.DigitalOffice.MessageService.Validation.EmailTemplate.Interfaces;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.MessageService.Business.Commands.EmailTemplate
{
  public class CreateEmailTemplateCommand : ICreateEmailTemplateCommand
  {
    private readonly IAccessValidator _accessValidator;
    private readonly ICreateEmailTemplateValidator _validator;
    private readonly IDbEmailTemplateMapper _mapper;
    private readonly IEmailTemplateRepository _repository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateEmailTemplateCommand(
      IAccessValidator accessValidator,
      ICreateEmailTemplateValidator validator,
      IDbEmailTemplateMapper mapper,
      IEmailTemplateRepository repository,
      IHttpContextAccessor httpContextAccessor)
    {
      _accessValidator = accessValidator;
      _validator = validator;
      _mapper = mapper;
      _repository = repository;
      _httpContextAccessor = httpContextAccessor;
    }

    public OperationResultResponse<Guid?> Execute(EmailTemplateRequest request)
    {
      if (!(_accessValidator.IsAdmin() || _accessValidator.HasRights(Rights.AddEditRemoveEmailTemplates)))
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        return new OperationResultResponse<Guid?>
        {
          Status = OperationResultStatusType.Failed,
          Errors = new() { "Not enough rights." }
        };
      }

      List<string> errors = new();

      if (!_validator.ValidateCustom(request, out errors))
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        return new OperationResultResponse<Guid?>
        {
          Status = OperationResultStatusType.Failed,
          Errors = errors
        };
      }

      OperationResultResponse<Guid?> response = new();

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
