using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.MessageService.Models.Dto.Helpers;
using LT.DigitalOffice.Models.Broker.Requests.Message;
using MassTransit;
using System.Threading.Tasks;

namespace LT.DigitalOffice.MessageService.Broker.Consumers
{
    public class UpdateSmtpCredentialsConsumer : IConsumer<IUpdateSmtpCredentialsRequest>
    {
        private object UpdateCredentials(IUpdateSmtpCredentialsRequest request)
        {
            SmtpCredentials.Host = request.Host;
            SmtpCredentials.Port = request.Port;
            SmtpCredentials.Email = request.Email;
            SmtpCredentials.Password = request.Password;
            SmtpCredentials.EnableSsl = request.EnableSsl;

            return true;
        }

        public async Task Consume(ConsumeContext<IUpdateSmtpCredentialsRequest> context)
        {
            var result = OperationResultWrapper.CreateResponse(UpdateCredentials, context.Message);

            await context.RespondAsync<IOperationResult<bool>>(result);
        }
    }
}
