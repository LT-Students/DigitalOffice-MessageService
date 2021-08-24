using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Db.Workspace.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.Models.Broker.Requests.Message;
using MassTransit;
using System.Threading.Tasks;

namespace LT.DigitalOffice.MessageService.Broker.Consumers
{
    public class CreateWorkspaceConsumer : IConsumer<ICreateWorkspaceRequest>
    {
        private readonly IWorkspaceRepository _workspaceRepository;
        private readonly IDbWorkspaceMapper _workspaceMapper;

        private bool CreateWorkspace(ICreateWorkspaceRequest request)
        {
            DbWorkspace newWorkspace = _workspaceMapper.Map(request);

            _workspaceRepository.Add(newWorkspace);

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
