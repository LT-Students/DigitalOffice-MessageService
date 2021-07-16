using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.MessageService.Broker.Consumers;
using LT.DigitalOffice.MessageService.Mappers.Db.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Db.Workspace.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.Models.Broker.Requests.Message;
using MassTransit;
using MassTransit.Testing;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LT.DigitalOffice.MessageService.Broker.UnitTests
{
    public class CreateWorkspaceConsumerTests
    {
        private AutoMocker _mocker;
        private InMemoryTestHarness _harness;
        private ConsumerTestHarness<CreateWorkspaceConsumer> _consumerTestHarness;
        private IRequestClient<ICreateWorkspaceRequest> _requestClient;

        private string _name;
        private Guid _creatorId;
        private Guid _workspaceId;
        private Guid _channelId;
        private Guid _workspaceUserId1;
        private Guid _workspaceUserId2;
        private List<Guid> _users;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _name = "Name";
            _creatorId = Guid.NewGuid();
            _workspaceId = Guid.NewGuid();
            _channelId = Guid.NewGuid();
            _workspaceUserId1 = Guid.NewGuid();
            _workspaceUserId2 = Guid.NewGuid();
            _users = new() { _creatorId, Guid.NewGuid() };
        }

        [SetUp]
        public void SetUp()
        {
            _mocker = new();
            _harness = new InMemoryTestHarness();
            _consumerTestHarness = _harness.Consumer(() =>
                _mocker.CreateInstance<CreateWorkspaceConsumer>());

            #region mock

            _mocker
                .Setup<IDbWorkspaceMapper, DbWorkspace>(x => x.Map(It.IsAny<ICreateWorkspaceRequest>()))
                .Returns(new DbWorkspace
                {
                    Id = _workspaceId
                });

            _mocker
                .Setup<IDbWorkspaceUserMapper, DbWorkspaceUser>(x => x.Map(_workspaceId, _creatorId, true))
                .Returns(new DbWorkspaceUser
                {
                    Id = _workspaceUserId1,
                    IsAdmin = true
                });

            _mocker
                .Setup<IDbWorkspaceUserMapper, DbWorkspaceUser>(x => x.Map(_workspaceId, _users[1], false))
                .Returns(new DbWorkspaceUser
                {
                    Id = _workspaceUserId2
                });

            _mocker
                .Setup<IDbChannelMapper, DbChannel>(x => x.Map(_workspaceId, _creatorId, "General", false))
                .Returns(new DbChannel
                {
                    Id = _channelId,
                    WorkspaceId = _workspaceId,
                    Name = "General",
                    IsPrivate = false
                });

            _mocker
                .Setup<IDbChannelUserMapper, DbChannelUser>(x => x.Map(_channelId, _workspaceUserId1, true))
                .Returns(new DbChannelUser
                {
                    Id = Guid.NewGuid(),
                    IsAdmin = true
                });

            _mocker
                .Setup<IDbChannelUserMapper, DbChannelUser>(x => x.Map(_channelId, _workspaceUserId2, false))
                .Returns(new DbChannelUser
                {
                    Id = Guid.NewGuid()
                });

            #endregion
        }

        [Test]
        public async Task ShouldCreateWorkspaceSuccessfuly()
        {
            var request = ICreateWorkspaceRequest.CreateObj(_name, _creatorId, _users);

            await _harness.Start();

            try
            {
                _requestClient = await _harness.ConnectRequestClient<ICreateWorkspaceRequest>();

                var response = await _requestClient.GetResponse<IOperationResult<bool>>(
                    request);

                Assert.IsTrue(response.Message.IsSuccess);
                Assert.IsNull(response.Message.Errors);
                Assert.IsTrue(response.Message.Body);
                Assert.True(_consumerTestHarness.Consumed.Select<ICreateWorkspaceRequest>().Any());
                Assert.True(_harness.Sent.Select<IOperationResult<bool>>().Any());
            }
            finally
            {
                await _harness.Stop();
            }
        }
    }
}
