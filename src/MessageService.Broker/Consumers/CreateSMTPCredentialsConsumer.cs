using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Db.Interfaces;
using LT.DigitalOffice.Models.Broker.Requests.Message;
using MassTransit;
using System.Threading.Tasks;

namespace LT.DigitalOffice.MessageService.Broker.Consumers
{
    public class CreateSMTPCredentialsConsumer : IConsumer<ICreateSMTPRequest>
    {
        private readonly ISMTPCredentialsRepository _repository;
        private readonly IDbSMTPCredentialsMapper _mapper;

        private object CreateSMTP(ICreateSMTPRequest request)
        {
            _repository.Create(_mapper.Map(request));

            return true;
        }

        public CreateSMTPCredentialsConsumer(
            ISMTPCredentialsRepository repository,
            IDbSMTPCredentialsMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<ICreateSMTPRequest> context)
        {
            var response = OperationResultWrapper.CreateResponse(CreateSMTP, context.Message);

            await context.RespondAsync<IOperationResult<bool>>(response);
        }
    }
}
