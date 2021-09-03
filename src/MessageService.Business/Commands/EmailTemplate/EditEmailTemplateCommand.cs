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
using LT.DigitalOffice.MessageService.Validation.Validators.EmailTemplate.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.MessageService.Business.Commands.EmailTemplate
{
  public class EditEmailTemplateCommand : IEditEmailTemplateCommand
  {
    private readonly IAccessValidator _accessValidator;
    private readonly IEmailTemplateRepository _repository;
    private readonly IEditEmailTemplateValidator _validator;
    private readonly IPatchDbEmailTemplateMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public EditEmailTemplateCommand(
        IAccessValidator accessValidator,
        IEmailTemplateRepository repository,
        IEditEmailTemplateValidator validator,
        IPatchDbEmailTemplateMapper mapper,
        IHttpContextAccessor httpContextAccessor)
    {
      _validator = validator;
      _repository = repository;
      _accessValidator = accessValidator;
      _mapper = mapper;
      _httpContextAccessor = httpContextAccessor;
    }

    public OperationResultResponse<bool> Execute(
      Guid emailTemplateId,
      JsonPatchDocument<EditEmailTemplateRequest> patch)
    {
      if (!(_accessValidator.IsAdmin() || _accessValidator.HasRights(Rights.AddEditRemoveEmailTemplates)))
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        return new OperationResultResponse<bool>
        {
          Status = OperationResultStatusType.Failed,
          Errors = new() { "Not enough rights." }
        };
      }

      if (!_validator.ValidateCustom(patch, out List<string> errors))
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        return new OperationResultResponse<bool>
        {
          Status = OperationResultStatusType.Failed,
          Errors = errors
        };
      }

      OperationResultResponse<bool> response = new();

      response.Body = _repository.Edit(emailTemplateId, _mapper.Map(patch));

      if (!response.Body)
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        response.Errors.Add("Bad request");
        response.Status = OperationResultStatusType.Failed;
        return response;
      }

      response.Status = OperationResultStatusType.FullSuccess;
      return response;
    }
  }
}
