using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Db.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.WorkspaceMappers.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.Models.Broker.Requests.Message;
using MassTransit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LT.DigitalOffice.MessageService.Broker.Consumers
{
    public class CreateWorkspaceConsumer : IConsumer<ICreateWorkspaceRequest>
    {
        private const string DefaultMainChannelName = "General";

        private readonly IWorkspaceRepository _workspaceRepository;
        private readonly IWorkspaceUserRepository _workspaceUserRepository;
        private readonly IChannelRepository _channelRepository;
        private readonly IChannelUserRepository _channelUserRepository;
        private readonly IDbWorkspaceMapper _workspaceMapper;
        private readonly IDbWorkspaceUserMapper _workspaceUserMapper;
        private readonly IDbChannelMapper _channelMapper;
        private readonly IDbChannelUserMapper _channelUserMapper;

        private bool CreateWorkspace(ICreateWorkspaceRequest request)
        {
            DbWorkspace newWorkspace = _workspaceMapper.Map(request);
            IEnumerable<DbWorkspaceUser> workspaceUsers = request.Users?.Select(userId => _workspaceUserMapper.Map(newWorkspace.Id, userId, userId == request.CreaterId));
            DbChannel newChannel = _channelMapper.Map(newWorkspace.Id, request.CreaterId, DefaultMainChannelName, false);
            IEnumerable<DbChannelUser> channelUsers = workspaceUsers?.Select(wu => _channelUserMapper.Map(newChannel.Id, wu.Id, wu.UserId == request.CreaterId));

            _workspaceRepository.Add(newWorkspace);
            if (workspaceUsers != null)
            {
                _workspaceUserRepository.AddRange(workspaceUsers);
            }
            _channelRepository.Add(newChannel);
            if (channelUsers != null)
            {
                _channelUserRepository.AddRange(channelUsers);
            }
            return true;
        }

        public CreateWorkspaceConsumer(
            IWorkspaceRepository workspaceRepository,
            IWorkspaceUserRepository workspaceUserRepository,
            IChannelRepository channelRepository,
            IChannelUserRepository channelUserRepository,
            IDbWorkspaceMapper workspaceMapper,
            IDbWorkspaceUserMapper workspaceUserMapper,
            IDbChannelMapper channelMapper,
            IDbChannelUserMapper channelUserMapper)
        {
            _workspaceRepository = workspaceRepository;
            _workspaceUserRepository = workspaceUserRepository;
            _channelRepository = channelRepository;
            _channelUserRepository = channelUserRepository;
            _workspaceMapper = workspaceMapper;
            _workspaceUserMapper = workspaceUserMapper;
            _channelMapper = channelMapper;
            _channelUserMapper = channelUserMapper;
        }

        public async Task Consume(ConsumeContext<ICreateWorkspaceRequest> context)
        {
            var response = OperationResultWrapper.CreateResponse(CreateWorkspace, context.Message);

            await context.RespondAsync<IOperationResult<bool>>(response);
        }
    }
}
