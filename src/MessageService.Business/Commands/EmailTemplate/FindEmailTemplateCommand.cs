using System.Linq;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.MessageService.Business.Commands.EmailTemplate.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Models.Interfaces;
using LT.DigitalOffice.MessageService.Models.Dto.Models.Emails;

namespace LT.DigitalOffice.MessageService.Business.Commands.EmailTemplate
{
  public class FindEmailTemplateCommand : IFindEmailTemplateCommand
  {
    private readonly IEmailTemplateRepository _repository;
    private readonly IEmailTemplateInfoMapper _mapper;

    public FindEmailTemplateCommand(
      IEmailTemplateRepository repository,
      IEmailTemplateInfoMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    public FindResultResponse<EmailTemplateInfo> Execute(
      int skipCount,
      int takeCount,
      bool includeDeactivated)
    {
      FindResultResponse<EmailTemplateInfo> response = new();

      response.Body = _repository
        .Find(skipCount, takeCount, out int totalCount, response.Errors, includeDeactivated)
        .Select(_mapper.Map)
        .ToList();

      response.Status = response.Errors.Any() ?
        OperationResultStatusType.Failed :
        OperationResultStatusType.FullSuccess;

      response.TotalCount = totalCount;

      return response;
    }
  }
}
