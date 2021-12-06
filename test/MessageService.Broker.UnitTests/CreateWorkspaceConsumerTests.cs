namespace LT.DigitalOffice.MessageService.Broker.UnitTests
{
  /*public class CreateWorkspaceConsumerTests
  {
      private AutoMocker _mocker;
      private InMemoryTestHarness _harness;
      private ConsumerTestHarness<CreateWorkspaceConsumer> _consumerTestHarness;
      private IRequestClient<ICreateWorkspaceRequest> _requestClient;

      private string _name;
      private Guid _creatorId;
      private Guid _workspaceId;
      private List<Guid> _users;

      [OneTimeSetUp]
      public void OneTimeSetUp()
      {
          _name = "Name";
          _creatorId = Guid.NewGuid();
          _workspaceId = Guid.NewGuid();
          _users = new() { _creatorId, Guid.NewGuid() };
      }

      [SetUp]
      public void SetUp()
      {
          _mocker = new();
          _harness = new InMemoryTestHarness();
          _consumerTestHarness = _harness.Consumer(() =>
              _mocker.CreateInstance<CreateWorkspaceConsumer>());

          _mocker
              .Setup<IDbWorkspaceMapper, DbWorkspace>(x => x.Map(It.IsAny<ICreateWorkspaceRequest>()))
              .Returns(new DbWorkspace
              {
                  Id = _workspaceId
              });
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
  }*/
}
