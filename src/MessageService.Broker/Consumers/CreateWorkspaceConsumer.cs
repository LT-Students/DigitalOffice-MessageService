using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Db.Interfaces;
using LT.DigitalOffice.Models.Broker.Requests.Message;
using MassTransit;

namespace LT.DigitalOffice.MessageService.Broker.Consumers
{
  public class CreateWorkspaceConsumer : IConsumer<ICreateWorkspaceRequest>
  {
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IDbWorkspaceMapper _workspaceMapper;

    private async Task<bool> CreateWorkspace(ICreateWorkspaceRequest request)
    {
      await _workspaceRepository.CreateAsync(_workspaceMapper.Map(request.Name, request.Users, request.CreaterId));

      return true;
    }

    public CreateWorkspaceConsumer(
      IWorkspaceRepository workspaceRepository,
      IDbWorkspaceMapper workspaceMapper)
    {
      _workspaceRepository = workspaceRepository;
      _workspaceMapper = workspaceMapper;
    }

    public async Task Consume(ConsumeContext<ICreateWorkspaceRequest> context)
    {
      var response = OperationResultWrapper.CreateResponse(CreateWorkspace, context.Message);

      await context.RespondAsync<IOperationResult<bool>>(response);
    }
  }
}
