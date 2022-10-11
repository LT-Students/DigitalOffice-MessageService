using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Kernel.Validators.Interfaces;
using LT.DigitalOffice.MessageService.Business.Commands.Workspace.Interfaces;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Models.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace.Filters;
using LT.DigitalOffice.MessageService.Models.Dto.Responses.Workspace;

namespace LT.DigitalOffice.MessageService.Business.Commands.Workspace
{
  public class FindWorkspaceCommand : IFindWorkspaceCommand
  {
    private readonly IBaseFindFilterValidator _baseFindValidator;
    private readonly IWorkspaceRepository _repository;
    private readonly IShortWorkspaceInfoMapper _mapper;
    private readonly IResponseCreator _responseCreator;

    public FindWorkspaceCommand(
      IBaseFindFilterValidator baseFindValidator,
      IWorkspaceRepository repository,
      IShortWorkspaceInfoMapper mapper,
      IResponseCreator responseCreator)
    {
      _baseFindValidator = baseFindValidator;
      _repository = repository;
      _mapper = mapper;
      _responseCreator = responseCreator;
    }

    public async Task<FindResultResponse<ShortWorkspaceInfo>> ExecuteAsync(FindWorkspaceFilter filter)
    {
      FindResultResponse<ShortWorkspaceInfo> response = new();

      if (!_baseFindValidator.ValidateCustom(filter, out List<string> errors))
      {
        return _responseCreator.CreateFailureFindResponse<ShortWorkspaceInfo>(
          HttpStatusCode.BadRequest, errors);
      }

      (List<DbWorkspace> dbWorkspases, int totalCount) = await _repository.FindAsync(filter);

      response.Body = dbWorkspases
        .Select(_mapper.Map)
        .ToList();

      response.TotalCount = totalCount;

      if (response.Errors.Any())
      {
        response = _responseCreator.CreateFailureFindResponse<ShortWorkspaceInfo>(
          HttpStatusCode.BadRequest);
      }

      return response;
    }
  }
}
